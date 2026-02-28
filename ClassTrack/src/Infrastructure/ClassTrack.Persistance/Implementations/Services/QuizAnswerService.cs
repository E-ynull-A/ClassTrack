using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using ClassTrack.Domain.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class QuizAnswerService : IQuizAnswerService
    {
        private readonly IQuizAnswerRepository _quizAnswerRepository;
        private readonly IMapper _mapper;
        private readonly IQuestionRepository _questionRepository;
        private readonly IStudentQuizRepository _studentQuizRepository;
        private readonly ICurrentUserService _userService;
        private readonly IStudentService _studentService;

        public QuizAnswerService(IQuizAnswerRepository quizAnswerRepository,
                                  IMapper mapper,
                                  IQuestionRepository questionRepository,
                                  IStudentQuizRepository studentQuizRepository,
                                  ICurrentUserService userService,
                                  IStudentService studentService)
        {
            _quizAnswerRepository = quizAnswerRepository;
            _mapper = mapper;
            _questionRepository = questionRepository;
            _studentQuizRepository = studentQuizRepository;
            _userService = userService;
            _studentService = studentService;
        }

        public async Task<ICollection<GetQuizAnswerItemDTO>> GetAllByStudentIdAsync(long studentId,
                                                                                    long quizId,
                                                                                    int page,
                                                                                    int take)
        {
            return _mapper.Map<ICollection<GetQuizAnswerItemDTO>>(await _quizAnswerRepository
                                                                    .GetAll(take: take,
                                                                            page: page,
                                                                            function: x => x.StudentQuiz.StudentId == studentId
                                                                                  && x.StudentQuiz.QuizId == quizId,
                                                                            includes: [nameof(QuizAnswer.Question)]).ToListAsync());
        }

        public async Task<GetQuizAnswerDTO> GetByIdAsync(long id)
        {
            return _mapper.Map<GetQuizAnswerDTO>(await _quizAnswerRepository
                     .GetQueryable()
                     .Include(q => q.Question)
                     .ThenInclude(q => (q as ChoiceQuestion).Options)
        .FirstOrDefaultAsync(qa => qa.Id == id));
        }
        public async Task TakeAnExamAsync(PostQuizAnswerDTO answerDTO)
        {
            StudentQuiz? sqs = await _studentQuizRepository
                            .FirstOrDefaultAsync(sq => sq.Student.AppUserId
                                                    == _userService.GetUserId()
                                                    && sq.QuizId == answerDTO.QuizId, ["Quiz"]);

            if (sqs is null)
                throw new NotFoundException("This Quiz isn't Found!");

            if (DateTime.UtcNow < sqs.Quiz.StartTime.ToUniversalTime() || DateTime.UtcNow > sqs.Quiz.StartTime.Add(sqs.Quiz.Duration).ToUniversalTime())
            {
                throw new BusinessLogicException("The Quiz doesn't begin or already finished!!");
            }

            if (sqs.QuizStatus == QuizStatus.Submitted.ToString())
            {
                throw new BusinessLogicException("You already submitted!");
            }

            if (await _questionRepository.CountAsync(q => answerDTO.Answers.Select(a => a.QuestionId).Contains(q.Id)
                                                       && q.QuizId
                                                       == sqs.QuizId)
                                                       != answerDTO.Answers.Count())
            {
                throw new BadRequestException("There is an issue about Questions!!");
            }


            var questionIds = await _questionRepository.GetAll().OfType<ChoiceQuestion>()
                                                       .Where(x => answerDTO.Answers.Select(a => a.QuestionId).Contains(x.Id))
                                                       .Select(q => new
                                                       {
                                                           q.Id,
                                                           q.Point,
                                                           Options = q.Options.ToList()
                                                       })
                                                       .ToDictionaryAsync(q => q.Id, q => q);

            foreach (var answer in answerDTO.Answers)
            {
                if (answer.AnswerId is not null)
                {
                    if (questionIds.TryGetValue(answer.QuestionId, out var result))
                    {
                        Option? option = result.Options.FirstOrDefault(o => o.Id == answer.AnswerId);
                        if (option is not null)
                        {
                            if (option.IsCorrect)
                            {
                                sqs.TotalPoint += result.Point;
                            }
                        }
                        else
                        {
                            throw new NotFoundException("The Option not Found!!");
                        }
                    }
                    else
                    {
                        throw new NotFoundException("The Question not Found!!");
                    }
                }
                else if (answer.AnswerIds is not null)
                {
                    if (questionIds.TryGetValue(answer.QuestionId, out var result))
                    {
                        int correctCount = 0;

                        foreach (var optId in answer.AnswerIds)
                        {
                            Option? option = result.Options.FirstOrDefault(o => o.Id == optId);

                            if (option is not null)
                            {
                                if (option.IsCorrect)
                                {
                                    correctCount++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                throw new NotFoundException("The Option isn't Found!!");
                            }
                        }
                        if (correctCount == result.Options.Count(o => o.IsCorrect))
                        {
                            sqs.TotalPoint += result.Point;
                        }
                    }
                }
            }

            if (answerDTO.Answers.Any(a => a.AnswerText is not null))
                sqs.QuizStatus = QuizStatus.Submitted.ToString();
            else
                sqs.QuizStatus = QuizStatus.Finished.ToString();

            _studentQuizRepository.Update(sqs);


            ICollection<QuizAnswer> qAnswers = answerDTO.Answers.Select(a => new QuizAnswer
            {
                AnswerId = a.AnswerId,
                QuestionId = a.QuestionId,
                AnswerIds = a.AnswerIds?.ToList(),
                AnswerText = a.AnswerText,
                IsEvaluated = a.IsEvaluated,
            }).ToList();


            foreach (var qAns in qAnswers)
            {
                qAns.StudentQuizId = sqs.Id;

                if (qAns.AnswerId.HasValue)
                    qAns.IsEvaluated = true;

                if (qAns.AnswerIds is not null)
                    qAns.IsEvaluated = true;

                if (!qAns.AnswerId.HasValue
                    && qAns.AnswerText is null
                    && qAns.AnswerIds is null)
                    qAns.IsEvaluated = true;
            }

            _quizAnswerRepository.AddRange(qAnswers);

            await _quizAnswerRepository.SaveChangeAsync();
        }
        public async Task EvaluateAnswerAsync(long id, PutQuizAnswerDTO answerDTO)
        {
            QuizAnswer answer = await _quizAnswerRepository.GetByIdAsync(id, includes: ["Question"]);

            if (answer is null)
                throw new NotFoundException("The QuizAnswer isn't Found");

            if (answer.IsEvaluated)
                throw new ConflictException("The Question is already Evaulate!");

            StudentQuiz studentQuiz = await _studentQuizRepository.GetByIdAsync(answer.StudentQuizId, includes: ["Quiz"]);

            if (studentQuiz is null)
                throw new NotFoundException("The Quiz isn't Found in this Class");

            if (DateTime.UtcNow < studentQuiz.Quiz.StartTime.ToUniversalTime())
                throw new BusinessLogicException("The Quiz doesn't begin yet!");

            if (answer.Question.Point < answerDTO.Point)
                throw new BusinessLogicException("Pleace, don't exceed the Point Limit of the Question!");

            answer.IsEvaluated = true;
            _quizAnswerRepository.Update(answer);

            studentQuiz.TotalPoint += answerDTO.Point;

            if (!await _quizAnswerRepository.AllAsync(qa => qa.StudentQuizId == studentQuiz.Id && !qa.IsEvaluated))
            {
                studentQuiz.QuizStatus = QuizStatus.Finished.ToString();
            }            

            _studentQuizRepository.Update(studentQuiz);
            await _studentQuizRepository.SaveChangeAsync();

            await _studentService.CalculateAvgPoint(studentQuiz.StudentId, studentQuiz.Quiz.ClassRoomId);
        }
    }
}
