using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using ClassTrack.Persistance.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class QuizAnswerService : IQuizAnswerService
    {
        private readonly IQuizAnswerRepository _quizAnswerRepository;
        private readonly IMapper _mapper;
        private readonly IQuestionRepository _questionRepository;
        private readonly IStudentQuizRepository _studentQuizRepository;
        private readonly IPermissionService _permissionService;

        public QuizAnswerService(IQuizAnswerRepository quizAnswerRepository,
                                  IMapper mapper,
                                  IQuestionRepository questionRepository,
                                  IStudentQuizRepository studentQuizRepository,
                                  IPermissionService permissionService)
        {
            _quizAnswerRepository = quizAnswerRepository;
            _mapper = mapper;
            _questionRepository = questionRepository;
            _studentQuizRepository = studentQuizRepository;
            _permissionService = permissionService;
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

                StudentQuiz sqs = await _studentQuizRepository.GetByIdAsync(answerDTO.StudentQuizId, includes: ["Quiz"]);
            if (sqs is null)
                throw new Exception("This Quiz isn't Found!");

            if (DateTime.UtcNow < sqs.Quiz.StartTime && DateTime.UtcNow > sqs.Quiz.StartTime.Add(sqs.Quiz.Duration))
            {
                throw new Exception("The Quiz doesn't begin or already finished!!");
            }

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
                if (answer.AnswerId is null)
                    continue;

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

            sqs.QuizStatus = QuizStatus.Finished.ToString();

            _studentQuizRepository.Update(sqs);

            ICollection<QuizAnswer> qAnswers = _mapper.Map<ICollection<QuizAnswer>>(answerDTO.Answers);

            foreach (var qAns in qAnswers)
            {
                qAns.StudentQuizId = sqs.Id;

                if (qAns.AnswerId.HasValue)
                    qAns.IsEvaluated = true;

                if (!qAns.AnswerId.HasValue && qAns.AnswerText is null)
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
