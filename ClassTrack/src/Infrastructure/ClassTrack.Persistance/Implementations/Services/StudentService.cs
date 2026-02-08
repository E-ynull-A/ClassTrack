using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class StudentService : IStudentService
    {
        private readonly IClassRoomRepository _roomRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICurrentUserService _currentUser;

        public StudentService(IClassRoomRepository roomRepository,
                              IStudentRepository studentRepository,
                              ICurrentUserService currentUser)

        {
            _roomRepository = roomRepository;
            _studentRepository = studentRepository;
            _currentUser = currentUser;
        }

        public async Task JoinClassAsync(JoinClassRoomDTO classRoomDTO)
        {
            ClassRoom room = await _roomRepository.FirstOrDefaultAsync(r => r.PrivateKey == classRoomDTO.ClassKey, includes: ["StudentClasses.Student"]);

            if (room is null)
                throw new Exception("The Room isn't Found!!");

            string userId = _currentUser.GetUserId();

            if (userId is null)
                throw new Exception("There is an issue about Login!");

            if(!room.StudentClasses.Any(sc=>sc.Student.AppUserId == userId))
            {
                Student? student = await _studentRepository.FirstOrDefaultAsync(s => s.AppUserId == userId);

                StudentClassRoom sRoom = new StudentClassRoom();

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

        public async Task CalculateAvgPoint(long studentId,long classRoomId,decimal point)
        {
            Student student = await _studentRepository
                .GetByIdAsync(studentId, includes: [nameof(Student.StudentQuizes),
                                                   nameof(Student.StudentClasses)]);

            if (student is null)
                throw new Exception("Student not Found!");

            StudentClassRoom? studentClass = student.StudentClasses.FirstOrDefault(sc => sc.ClassRoomId == classRoomId);

            if (studentClass is null)
                throw new Exception("You are not the member of this class");

           //Quiz-ə totalPoint əlavə elə

            studentClass.AvgPoint += student.StudentQuizes.Average(sq => sq.TotalPoint);

            _studentRepository.Update(student);
            await _studentRepository.SaveChangeAsync();
        }
    }
}

