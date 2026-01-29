using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using ClassTrack.Persistance.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly AppDbContext _context;

        public QuizAnswerService(IQuizAnswerRepository quizAnswerRepository,
                                  IMapper mapper,
                                  IQuestionRepository questionRepository,
                                  IStudentQuizRepository studentQuizRepository,
                                  AppDbContext context)
        {
            _quizAnswerRepository = quizAnswerRepository;
            _mapper = mapper;
            _questionRepository = questionRepository;
            _studentQuizRepository = studentQuizRepository;
            _context = context;
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
            return _mapper.Map<GetQuizAnswerDTO>(await _context.QuizAnswers
               .Include(qa => qa.Question)
                   .ThenInclude(q => (q as ChoiceQuestion).Options)
               .FirstOrDefaultAsync(qa => qa.Id == id));
        }

        public async Task TakeAnExam(PostQuizAnswerDTO answerDTO)
        {
            if (!await _studentQuizRepository.AnyAsync(sq => sq.Id == answerDTO.StudentQuizId))
                throw new Exception("This Quiz isn't For You!");

            StudentQuiz sqs = await _studentQuizRepository.GetByIdAsync(answerDTO.StudentQuizId);
            if (sqs is null)
                throw new Exception("Empty!!!");

            if(sqs.QuizStatus == QuizStatus.Finished.ToString())
            {
                throw new Exception("You already submitted!");
            }   
        }



        
    }
}
