using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class QuestionService:IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository,IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<GetQuestionItemDTO>> GetAllAsync(int page,int take,params string[] includes)
        {
            return _mapper.Map<ICollection<GetQuestionItemDTO>>
                (await _questionRepository.GetAll(page: page,                                                    
                                            take: take,
                                            sort: x => x.Title,
                                            includes: [nameof(ChoiceQuestion.Options),"Quiz"]).ToListAsync());
        }
       
        
        public async Task<GetQuestionDTO> GetByIdAsync(long id)
        {
            Question question = await _questionRepository
                                        .GetByIdAsync(id, includes: ["Quiz.Class", "Options"]);

            if(question is null)
                throw new Exception("Question Not Found");

            return _mapper.Map<GetQuestionDTO>(question);       
        }



    }
}
