using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class QuizService:IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;

        public QuizService(
                           IQuizRepository quizRepository,
                           IMapper mapper)
        {        
            _quizRepository = quizRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<GetQuizItemDTO>> GetAllAsync(int page,int take,params string[] includes)
        {
           
            ICollection<Quiz> quizes = await _quizRepository
                                    .GetAll(page: page,
                                            take: take,
                                            includes:includes,
                                            sort:x=>x.CreatedAt)
                                    .ToListAsync();

            return _mapper.Map<ICollection<GetQuizItemDTO>>(quizes);
        }
        public async Task<GetQuizDTO> GetByIdAsync(long id)
        {
           Quiz quiz = await _quizRepository
                            .GetByIdAsync(id, includes: ["ChoiceQuestions.Options","OpenQuestions"]);

           return _mapper.Map<GetQuizDTO>(quiz);
        }
    }
}
