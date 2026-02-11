using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using ClassTrack.Persistance.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;
        private readonly IClassRoomRepository _roomRepository;
        private readonly IStudentRepository _studentRepository;


        public QuizService(
                           IQuizRepository quizRepository,
                           IMapper mapper,
                           IClassRoomRepository roomRepository,
                           IStudentRepository studentRepository)

        {
            _quizRepository = quizRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
            _studentRepository = studentRepository;
        }

        public async Task<ICollection<GetQuizItemDTO>> GetAllAsync(long classRoomId, int page, int take,params string[] includes)
        {
            ICollection<Quiz> quizes = await _quizRepository
                                    .GetAll(page: page,
                                            take: take,
                                            includes: includes,
                                            function:x=>x.ClassRoomId == classRoomId,
                                            sort: x => x.CreatedAt)
                                    .ToListAsync();
            if (quizes.Count == 0)
                return  new List<GetQuizItemDTO>();

            return _mapper.Map<ICollection<GetQuizItemDTO>>(quizes);
        }
        public async Task<GetQuizItemDTO> GetByIdAsync(long id)
        {

            Quiz quiz = await _quizRepository.GetByIdAsync(id);

            if (quiz is null)
                throw new Exception("The Quiz not Found!");

            return _mapper.Map<GetQuizItemDTO>(quiz);
        }
        public async Task<GetQuizDTO> GetByIdDetailAsync(long id)
        {

            Quiz quiz = await _quizRepository
                             .GetByIdAsync(id, includes: ["ChoiceQuestions.Options", "OpenQuestions"]);

            if (DateTime.UtcNow < quiz.StartTime
                && DateTime.UtcNow > quiz.StartTime.Add(quiz.Duration))
            {
                throw new Exception("You can enter The Quiz only after the Starting Quiz " +
                                    "and Before the Quiz Ending!!");
            }

            return _mapper.Map<GetQuizDTO>(quiz);
        }
        public async Task CreateQuizAsync(PostQuizDTO postQuiz)
        {

            if (!await _roomRepository.AnyAsync(r => r.Id == postQuiz.ClassRoomId))
            {
                throw new Exception("The ClassRoom isn't Found!");
            }

            int totalCount = 0;
            if (postQuiz.ChoiceQuestions is not null)
                totalCount += postQuiz.ChoiceQuestions.Count;
            if (postQuiz.OpenQuestions is not null)
                totalCount += postQuiz.OpenQuestions.Count;

            if (totalCount > 200)
            {
                throw new Exception("The Count of Quiz is so high");
            }

            if (await _quizRepository.AnyAsync(q => q.Name == postQuiz.Name &&
                                             q.ClassRoomId != postQuiz.ClassRoomId))
            {
                throw new Exception("Don't Create the Quiz with the same Name in this Class!");
            }

            Quiz created = _mapper.Map<Quiz>(postQuiz);

            ICollection<Student> students = await _studentRepository
                                                        .GetAll(function: x => x.StudentClasses
                                                                    .Select(sc => sc.ClassRoomId)
                                                                    .Contains(postQuiz.ClassRoomId))
                                                        .ToListAsync();

            created.StudentQuizes = students
                                        .Select(s => new StudentQuiz
                                        {
                                            StudentId = s.Id,
                                            QuizId = created.Id,
                                            QuizStatus = QuizStatus.Pending.ToString()
                                        })
                                        .ToList();


            _quizRepository.Add(created);

            await _quizRepository.SaveChangeAsync();
        }
        public async Task UpdateQuizAsync(long id, PutQuizDTO putQuiz)
        {
            Quiz edited = await _quizRepository.GetByIdAsync(id);

            if (edited is null)
                throw new Exception("The Quiz isn't Found!");

            if (!await _roomRepository.AnyAsync(r => r.Id == putQuiz.ClassRoomId))
                throw new Exception("The ClassRoom isn't Found!");

            _getAllowQuizModify(edited);

            if (await _quizRepository.AnyAsync(q => q.Name == edited.Name &&
                                                 q.ClassRoomId != edited.ClassRoomId))          
                throw new Exception("Don't Update the Quiz to the same Name in this Class!");
            
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
        private void _getAllowQuizModify(Quiz quiz)
        {
            if (quiz.StartTime <= DateTime.UtcNow && quiz.StartTime.Add(quiz.Duration) > DateTime.UtcNow)
                throw new Exception("Couldn't Modify The Quiz during Quiz Time!");

            if (quiz.StartTime.Add(quiz.Duration) < DateTime.UtcNow)
                throw new Exception("Couldn't Modify The Quiz after the Quiz!");
        }
    }
}
