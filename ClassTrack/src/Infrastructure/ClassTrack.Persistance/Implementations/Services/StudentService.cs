using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using ClassTrack.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class StudentService : IStudentService
    {
        private readonly IClassRoomRepository _roomRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IStudentQuizRepository _studentQuiz;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ITaskWorkRepository _taskRepository;
        private readonly ICacheService _cacheService;
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;

        public StudentService(IClassRoomRepository roomRepository,
                              IStudentRepository studentRepository,
                              ICurrentUserService currentUser,
                              IStudentQuizRepository studentQuiz,
                              ITeacherRepository teacherRepository,
                              ITaskWorkRepository taskRepository,
                              ICacheService cacheService,
                              IQuizRepository quizRepository,
                              IMapper mapper)

        {
            _roomRepository = roomRepository;
            _studentRepository = studentRepository;
            _currentUser = currentUser;
            _studentQuiz = studentQuiz;
            _teacherRepository = teacherRepository;
            _taskRepository = taskRepository;
            _cacheService = cacheService;
            _quizRepository = quizRepository;
            _mapper = mapper;
        }



        public async Task<ICollection<GetSimpleStudentItemDTO>> GetBriefAllAsync(long classRoomId, int page, int take)
        {
            if (!await _roomRepository.AnyAsync(r => r.Id == classRoomId))
                throw new NotFoundException("Class Room not Found!");

            return _mapper.Map<ICollection<GetSimpleStudentItemDTO>>(_studentRepository.GetAll(page: page,
                                                                                               take: take,
                                                                                               includes: [nameof(Student.AppUser)],
                                                                                               function: x => x.StudentClasses.Select(sc => sc.ClassRoomId).Contains(classRoomId),
                                                                                               sort: x => x.AppUser.Name));
        }
        public async Task<ICollection<GetStudentItemDTO>> GetAllAsync(long classRoomId, int page, int take)
        {
            if (!await _roomRepository.AnyAsync(r => r.Id == classRoomId))
                throw new NotFoundException("Class Room not Found!");

            return _mapper.Map<ICollection<GetStudentItemDTO>>(_studentRepository.GetAll(page: page,
                                                                                         take: take,
                                                                                         includes: [nameof(Student.AppUser),
                                                                                                   nameof(Student.StudentClasses)],
                                                                                         function: x => x.StudentClasses.Select(sc => sc.ClassRoomId)
                                                                                                                        .Contains(classRoomId),
                                                                                         sort: x => x.AppUser.Name));
        }
        public async Task<ICollection<GetStudentQuizResultDTO>> GetStudentResultAsync(long classRoomId)
        {
            string userId = _currentUser.GetUserId();

            if (userId is null)
                throw new NotFoundException("User not Found!");

            return _mapper.Map<ICollection<GetStudentQuizResultDTO>>(await _quizRepository.GetAll(includes: ["StudentQuizes"],
                                                                   function:q=>q.ClassRoomId == classRoomId 
                                                                   && q.StudentQuizes.Select(sq=>sq.Student.AppUserId)
                                                                   .Contains(userId) && q.StudentQuizes.Select(sq=>sq.QuizStatus).Contains("Finished")).ToListAsync());
        }
        public async Task JoinClassAsync(JoinClassRoomDTO classRoomDTO)
        {
            ClassRoom? room = await _roomRepository.FirstOrDefaultAsync(r => r.PrivateKey == classRoomDTO.ClassKey,
                                                                            includes: ["StudentClasses.Student"]);

            if (room is null)
                throw new NotFoundException("The Room isn't Found!!");

            string userId = _currentUser.GetUserId();
            string userRole = _currentUser.GetUserRole();
            if (userRole == UserRole.Admin.ToString())
                throw new BadRequestException("Admin can not create Class Room");

            if (await _roomRepository.AnyAsync(r => (r.PrivateKey == classRoomDTO.ClassKey)
               && (r.StudentClasses.Any(sc => sc.Student.AppUserId == userId)
                  || r.TeacherClasses.Any(tc => tc.Teacher.AppUserId == userId))))
            {
                throw new ConflictException("You already member of this Class");
            }

            if (userId is null)
                throw new BadRequestException("There is an issue about Login!");

            if (!room.StudentClasses.Any(sc => sc.Student.AppUserId == userId))
            {
                Student? student = await _studentRepository.FirstOrDefaultAsync(s => s.AppUserId == userId);

                StudentClassRoom sRoom = new StudentClassRoom();
                sRoom.JoinedAt = DateTime.UtcNow;

                if (student is null)
                {
                    sRoom.Student = new Student { AppUserId = userId };
                }
                else
                {
                    sRoom.StudentId = student.Id;
                }

                room.StudentClasses.Add(sRoom);

                _roomRepository.Update(room);
                await _roomRepository.SaveChangeAsync();
            }
        }
        public async Task KickAsync(long studentId, long classRoomId)
        {


            ClassRoom? room = await _roomRepository.GetByIdAsync(classRoomId);

            if (room is null)
                throw new NotFoundException("Class Room not Found");

            Student? student = await _studentRepository.GetByIdAsync(studentId, includes: ["StudentClasses"]);

            if (student is null)
                throw new NotFoundException("Student Not Found");

            StudentClassRoom? studentRoom = student.StudentClasses.FirstOrDefault(sc => sc.ClassRoomId == classRoomId);

            if (studentRoom is null)
            {
                throw new NotFoundException("The Student not Found in this Class Room");
            }

            student.StudentClasses.Remove(studentRoom);
            _studentRepository.Update(student);
            await _roomRepository.SaveChangeAsync();

        }
        public async Task RequestLeaveAsync(LeaveTokenDTO tokenDTO)
        {
            string? value = await _cacheService.GetAsync<string>(tokenDTO.Token);

            if (value is null)
                throw new BadRequestException("Invalid Token Response");

            string[] strs = value.Split(":");

            string userId = strs[0];
            int classRoomId = int.Parse(strs[1]);

            await _roomRepository.BreakStudentClassRoomAsync(classRoomId,userId);
            await _roomRepository.SaveChangeAsync();
            await _cacheService.RemoveAsync(tokenDTO.Token);
        }
        public async Task PromoteAsync(long studentId, long classRoomId)
        {
            ClassRoom? room = await _roomRepository.GetByIdAsync(classRoomId);

            if (room is null)
                throw new NotFoundException("Class Room not Found");

            Student? student = await _studentRepository.GetByIdAsync(studentId, includes: ["StudentClasses", "AppUser"]);

            if (student is null)
                throw new NotFoundException("Student Not Found");

            StudentClassRoom? studentRoom = student.StudentClasses.FirstOrDefault(sc => sc.ClassRoomId == classRoomId);

            if (studentRoom is null)
            {
                throw new NotFoundException("The Student not Found in this Class Room");
            }

            student.StudentClasses.Remove(studentRoom);
            _studentRepository.Update(student);


            Teacher? promotedStudent = await _teacherRepository.GetTeacherByUserIdAsync(student.AppUserId, ["TeacherClassRooms"]);

            if (promotedStudent is null)
            {
                promotedStudent = new Teacher { AppUserId = student.AppUserId };
                _teacherRepository.Add(promotedStudent);
            }

            promotedStudent.TeacherClassRooms.Add(new TeacherClassRoom { ClassRoomId = classRoomId });
            _teacherRepository.Update(promotedStudent);
            await _teacherRepository.SaveChangeAsync();
        }
        public async Task CalculateAvgPoint(long studentId, long classRoomId)
        {
            Student student = await _studentRepository
                .GetByIdAsync(studentId, includes: [nameof(Student.StudentQuizes),
                                                   nameof(Student.StudentClasses)]);

            if (student is null)
                throw new NotFoundException("Student not Found!");

            StudentClassRoom? studentClass = student.StudentClasses.FirstOrDefault(sc => sc.ClassRoomId == classRoomId);

            if (studentClass is null)
                throw new NotFoundException("You are not the member of this class");

            decimal fullQuizPoint = await _studentQuiz.AverageDetailAsync(x => (x.TotalPoint / x.Quiz.FullPoint) * 0.6m,
                                                                    x => x.Quiz.ClassRoomId == classRoomId &&
                                                                    x.StudentId == studentId);

            decimal avgTaskPoint = await _taskRepository.GetStudentTaskPointAvgAsync(classRoomId,studentId)*0.4m;

            studentClass.AvgPoint = avgTaskPoint + fullQuizPoint;

            _studentRepository.Update(student);
            await _studentRepository.SaveChangeAsync();
        }
    }
}

