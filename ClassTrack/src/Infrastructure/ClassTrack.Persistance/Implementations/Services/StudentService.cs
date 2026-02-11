using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class StudentService : IStudentService
    {
        private readonly IClassRoomRepository _roomRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IStudentQuizRepository _studentQuiz;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public StudentService(IClassRoomRepository roomRepository,
                              IStudentRepository studentRepository,
                              ICurrentUserService currentUser,
                              IStudentQuizRepository studentQuiz,
                              ITeacherRepository teacherRepository,
                              IMapper mapper)

        {
            _roomRepository = roomRepository;
            _studentRepository = studentRepository;
            _currentUser = currentUser;
            _studentQuiz = studentQuiz;
            _teacherRepository = teacherRepository;
            _mapper = mapper;
        }


        public async Task<ICollection<GetStudentItemDTO>> GetAllAsync(long classRoomId, int page, int take)
        {
            if (!await _roomRepository.AnyAsync(r => r.Id == classRoomId))
                throw new Exception("Class Room not Found!");

            return _mapper.Map<ICollection<GetStudentItemDTO>>(_studentRepository.GetAll(page: page,
                                                                                         take: take,
                                                                                         includes: [nameof(Student.AppUser),
                                                                                                   nameof(Student.StudentClasses)],
                                                                                         function: x => x.StudentClasses.Select(sc => sc.ClassRoomId).Contains(classRoomId),
                                                                                         sort: x => x.AppUser.Name));



        }
        public async Task JoinClassAsync(JoinClassRoomDTO classRoomDTO)
        {
            ClassRoom room = await _roomRepository.FirstOrDefaultAsync(r => r.PrivateKey == classRoomDTO.ClassKey,
                                                                            includes: ["StudentClasses.Student"]);

            if (room is null)
                throw new Exception("The Room isn't Found!!");

            string userId = _currentUser.GetUserId();

            if (await _roomRepository.AnyAsync(r => (r.PrivateKey == classRoomDTO.ClassKey)
               && (r.StudentClasses.Any(sc => sc.Student.AppUserId == userId)
                  || r.TeacherClasses.Any(tc => tc.Teacher.AppUserId == userId))))
            {
                throw new Exception("You already member of this Class");
            }

            if (userId is null)
                throw new Exception("There is an issue about Login!");

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
                throw new Exception("Class Room not Found");

            Student? student = await _studentRepository.GetByIdAsync(studentId,includes: ["StudentClasses"]);

            if (student is null)
                throw new Exception("Student Not Found");

            StudentClassRoom? studentRoom = student.StudentClasses.FirstOrDefault(sc => sc.ClassRoomId == classRoomId);

            if (studentRoom is null)
            {
                throw new Exception("The Student not Found in this Class Room");
            }

            student.StudentClasses.Remove(studentRoom);
            _studentRepository.Update(student);
            await _roomRepository.SaveChangeAsync();

        }
        public async Task PromoteAsync(long studentId, long classRoomId)
        {
            ClassRoom? room = await _roomRepository.GetByIdAsync(classRoomId);

            if (room is null)
                throw new Exception("Class Room not Found");

            Student? student = await _studentRepository.GetByIdAsync(studentId, includes: ["StudentClasses","AppUser"]);

            if (student is null)
                throw new Exception("Student Not Found");

            StudentClassRoom? studentRoom = student.StudentClasses.FirstOrDefault(sc => sc.ClassRoomId == classRoomId);

            if (studentRoom is null)
            {
                throw new Exception("The Student not Found in this Class Room");
            }

            student.StudentClasses.Remove(studentRoom);
            _studentRepository.Update(student);
            

            Teacher? promotedStudent = await _teacherRepository.GetTeacherByUserIdAsync(student.AppUserId,["TeacherClassRooms"]);

            if (promotedStudent is null)
            {
                promotedStudent = new Teacher{ AppUserId = student.AppUserId };
                _teacherRepository.Add(promotedStudent);
            }

            promotedStudent.TeacherClassRooms.Add(new TeacherClassRoom { ClassRoomId = classRoomId });
            _teacherRepository.Update(promotedStudent);
            await _teacherRepository.SaveChangeAsync();
        }
        public async Task CalculateAvgPoint(long studentId, long classRoomId, decimal point)
        {
            Student student = await _studentRepository
                .GetByIdAsync(studentId, includes: [nameof(Student.StudentQuizes),
                                                   nameof(Student.StudentClasses)]);

            if (student is null)
                throw new Exception("Student not Found!");

            StudentClassRoom? studentClass = student.StudentClasses.FirstOrDefault(sc => sc.ClassRoomId == classRoomId);

            if (studentClass is null)
                throw new Exception("You are not the member of this class");


            decimal fullQuizPoint = await _studentQuiz.AverageAsync(x => (x.TotalPoint / x.Quiz.FullPoint) * 1.5m,
                                                                    x => x.Quiz.ClassRoomId == classRoomId &&
                                                                    x.StudentId == studentId);

            studentClass.AvgPoint += fullQuizPoint;

            _studentRepository.Update(student);
            await _studentRepository.SaveChangeAsync();
        }
    }
}

