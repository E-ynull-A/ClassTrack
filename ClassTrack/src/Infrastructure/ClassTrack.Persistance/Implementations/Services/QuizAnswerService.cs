using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class QuizAnswerService : IQuizAnswerService
    {
        private readonly IQuizAnswerRepository _quizAnswerRepository;
        private readonly IMapper _mapper;
        private readonly IQuestionRepository _questionRepository;
        private readonly IStudentQuizRepository _studentQuizRepository;
        private readonly IPermissionService _permissionService;
        private readonly ICurrentUserService _userService;

        public QuizAnswerService(IQuizAnswerRepository quizAnswerRepository,
                                  IMapper mapper,
                                  IQuestionRepository questionRepository,
                                  IStudentQuizRepository studentQuizRepository,
                                  IPermissionService permissionService,
                                  ICurrentUserService userService)
        {
            _quizAnswerRepository = quizAnswerRepository;
            _mapper = mapper;
            _questionRepository = questionRepository;
            _studentQuizRepository = studentQuizRepository;
            _permissionService = permissionService;
            _userService = userService;
        }

        public async Task<ICollection<GetQuizAnswerItemDTO>> GetAllByStudentIdAsync(long studentId, int page, int take)
        {
            return _mapper.Map<ICollection<GetQuizAnswerItemDTO>>(await _quizAnswerRepository
                                                                    .GetAll(take: take,
                                                                            page: page,
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
                throw new Exception("This Quiz isn't Found!");

            //if (DateTime.UtcNow < sqs.Quiz.StartTime || DateTime.UtcNow > sqs.Quiz.StartTime.Add(sqs.Quiz.Duration))
            //{
            //    throw new Exception("The Quiz doesn't begin or already finished!!");
            //}

            if (sqs.QuizStatus == QuizStatus.Finished.ToString())
            {
                throw new Exception("You already submitted!");
            }

            if (await _questionRepository.CountAsync(q => answerDTO.Answers.Select(a => a.QuestionId).Contains(q.Id)
                                                       && q.QuizId
                                                       == sqs.QuizId)
                                                       != answerDTO.Answers.Count())
            {
                throw new Exception("There is an issue about Questions!!");
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
                            throw new Exception("The Option isn't Found!!");
                        }
                    }
                    else
                    {
                        throw new Exception("The Question isn't Found!!");
                    }
                }
                else if(answer.AnswerIds is not null)
                {                    
                    if(questionIds.TryGetValue(answer.QuestionId,out var result))
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
                                throw new Exception("The Option isn't Found!!");
                            }
                        }
                        if(correctCount == result.Options.Count(o => o.IsCorrect))
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

                if (!  qAns.AnswerId.HasValue 
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
                throw new Exception("The QuizAnswer isn't Found");

            if (answer.IsEvaluated)
                throw new Exception("The Question is already Evaulate!");

            StudentQuiz studentQuiz = await _studentQuizRepository.GetByIdAsync(answer.StudentQuizId, includes: ["Quiz"]);

            if (studentQuiz is null)
                throw new Exception("The Quiz isn't Found in this Class");

            if (DateTime.UtcNow < studentQuiz.Quiz.StartTime)
                throw new Exception("The Quiz doesn't begin yet!");

            if (answer.Question.Point < answerDTO.Point)
                throw new Exception("Pleace, don't exceed the Point Limit of the Question!");

            answer.IsEvaluated = true;
            _quizAnswerRepository.Update(answer);

            studentQuiz.TotalPoint += answerDTO.Point;
            _studentQuizRepository.Update(studentQuiz);

            await _quizAnswerRepository.SaveChangeAsync();
        }

    

    }
}
