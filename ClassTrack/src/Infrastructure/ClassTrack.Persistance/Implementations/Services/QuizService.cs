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
    internal class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;
        private readonly IClassRoomRepository _roomRepository;
        private readonly IQuestionService _questionService;
        private readonly IQuestionRepository _questionRepository;

        public QuizService(
                           IQuizRepository quizRepository,
                           IMapper mapper,
                           IClassRoomRepository roomRepository,
                           IQuestionService questionService,
                           IQuestionRepository questionRepository)
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
            _questionService = questionService;
            _questionRepository = questionRepository;
        }

        public async Task<ICollection<GetQuizItemDTO>> GetAllAsync(int page, int take, params string[] includes)
        {

            ICollection<Quiz> quizes = await _quizRepository
                                    .GetAll(page: page,
                                            take: take,
                                            includes: includes,
                                            sort: x => x.CreatedAt)
                                    .ToListAsync();

            return _mapper.Map<ICollection<GetQuizItemDTO>>(quizes);
        }
        public async Task<GetQuizDTO> GetByIdAsync(long id)
        {
            Quiz quiz = await _quizRepository
                             .GetByIdAsync(id, includes: ["ChoiceQuestions.Options", "OpenQuestions"]);

            return _mapper.Map<GetQuizDTO>(quiz);
        }


        public async Task CreateQuizAsync(PostQuizDTO postQuiz)
        {
            if (!await _roomRepository.AnyAsync(r => r.Id == postQuiz.ClassRoomId))
            {
                throw new Exception("The ClassRoom isn't Found!");
            }

            if (await _quizRepository.AnyAsync(q => q.Name == postQuiz.Name &&
                                                 q.ClassRoomId != postQuiz.ClassRoomId))
            {
                throw new Exception("Don't Create the Quiz with the same Name in this Class!");
            }

            _quizRepository.Add(_mapper.Map<Quiz>(postQuiz));

            await _quizRepository.SaveChangeAsync();
        }


        public async Task UpdateQuizAsync(long id, PutQuizDTO putQuiz)
        {
            Quiz edited = await _quizRepository.GetByIdAsync(id);

            if (await _quizRepository.AnyAsync(q => q.Name == edited.Name &&
                                                 q.ClassRoomId != edited.ClassRoomId))
            {
                throw new Exception("Don't Update the Quiz to the same Name in this Class!");
            }

            GetAllowQuizModify(edited);

            _quizRepository.Update(_mapper.Map(putQuiz, edited));       

            await _quizRepository.SaveChangeAsync();
        }

        public async Task DeleteQuizAsync(long id)
        {
            Quiz? deleted = await _quizRepository.GetByIdAsync(id);

            if (deleted == null)
                throw new Exception("The Quiz isn't Found!");
          
            _quizRepository.Delete(deleted);
            await _quizRepository.SaveChangeAsync();
        }


        private void GetAllowQuizModify(Quiz quiz)
        {
            if (quiz.StartTime <= DateTime.UtcNow && quiz.StartTime.Add(quiz.Duration) > DateTime.UtcNow)
                throw new Exception("Couldn't Modify The Quiz during Quiz Time!");

            if (quiz.StartTime.Add(quiz.Duration) < DateTime.UtcNow)
                throw new Exception("Couldn't Modify The Quiz after the Quiz!");
        }



    }
}
