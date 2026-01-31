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
        private readonly IHttpContextAccessor _accessor;
        private readonly IClassRoomRepository _roomRepository;
        private readonly IStudentRepository _studentRepository;

        public StudentService(IHttpContextAccessor accessor,
                              IClassRoomRepository roomRepository,
                              IStudentRepository studentRepository)

        {
            _accessor = accessor;
            _roomRepository = roomRepository;
            _studentRepository = studentRepository;
        }

        public async Task JoinClassAsync(JoinClassRoomDTO classRoomDTO)
        {
            ClassRoom room = await _roomRepository.FirstOrDefaultAsync(r => r.PrivateKey == classRoomDTO.ClassKey, includes: ["StudentClasses.Student"]);

            if (room is null)
                throw new Exception("The Room isn't Found!!");

            string userId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

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
    }
}

