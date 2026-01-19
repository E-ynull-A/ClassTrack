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
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;
        private readonly IOptionRepository _optionRepository;

        public QuestionService(IQuestionRepository questionRepository
                                ,IQuizRepository quizRepository
                                    ,IMapper mapper,
                                IOptionRepository optionRepository)
        {
            _questionRepository = questionRepository;
            _quizRepository = quizRepository;
            _mapper = mapper;
            _optionRepository = optionRepository;
        }

        public async Task<ICollection<GetQuestionItemDTO>> GetAllAsync(int page,int take,params string[] includes)
        {

            ICollection<Question> questions = await _questionRepository.GetAll(page: page,
                                            take: take,
                                            sort: x => x.CreatedAt,
                                            isIgnore:true,
                                            includes: ["Options","Quiz"]).ToListAsync();

            return _mapper.Map<ICollection<GetQuestionItemDTO>>(questions);
        }
       
        
        public async Task<GetQuestionDTO> GetByIdAsync(long id)
        {
            Question question = await _questionRepository
                                        .GetByIdAsync(id, includes: ["Quiz.Class", "Options"]);

            if(question is null)
                throw new Exception("Question Not Found");

            return _mapper.Map<GetQuestionDTO>(question);       
        }

        public async Task CreateChoiceQuestionAsync(PostChoiceQuestionDTO postChoice)
        {
            if(!await _quizRepository.AnyAsync(q=>q.Id == postChoice.QuizId))
            {
                throw new Exception("The Quiz doesn't exist!");
            }

            Quiz quiz = await _quizRepository.GetByIdAsync(postChoice.QuizId);

            if (quiz.StartTime <= DateTime.UtcNow && quiz.StartTime.Add(quiz.Duration) > DateTime.UtcNow)
                throw new Exception("Couldn't Add New Question during an Quiz Interval!");

            _optionRepository.Add(_mapper.Map<Option>(postChoice.Options));

            ChoiceQuestion questionDTO = _mapper.Map<ChoiceQuestion>(postChoice);

            _questionRepository.Add(questionDTO);

            await _questionRepository.SaveChangeAsync();
        }



    }
}
