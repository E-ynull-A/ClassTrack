using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class QuizAnswerService:IQuizAnswerService
    {
        private readonly IQuizAnswerRepository _quizAnswerRepository;
        private readonly IMapper _mapper;

        public QuizAnswerService(IQuizAnswerRepository quizAnswerRepository,
                                  IMapper mapper)
        {
            _quizAnswerRepository = quizAnswerRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<GetQuizAnswerItemDTO>> GetAllByStudentIdAsync(long studentId,int page, int take)
        {
            return _mapper.Map<ICollection<GetQuizAnswerItemDTO>>(await _quizAnswerRepository
                                                                    .GetAll(take:take,
                                                                            page:page,
                                                                            includes: [nameof(QuizAnswer.Question)]).ToListAsync());
        }
    }
}
