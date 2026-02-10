using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using ClassTrack.Persistance.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class ClassRoomService : IClassRoomService
    {
        private readonly IClassRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ICurrentUserService _currentUser;

        public ClassRoomService(IClassRoomRepository roomRepository,
                                IMapper mapper,
                                ITeacherRepository teacherRepository,
                                ICurrentUserService currentUser)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _teacherRepository = teacherRepository;
            _currentUser = currentUser;
        }

        public async Task<ICollection<GetClassRoomItemDTO>> GetAllAsync(int page,
                                                                        int take)
        {
            string userId = _currentUser.GetUserId();
            string userRole = string.Empty;
            try
            {
                userRole = _currentUser.GetUserRole();
            }
            catch (Exception e)
            {
                throw e;
            }

            var getClasses = _roomRepository.GetAll(page: page, take: take,sort:x=>x.Name);

            if (userId is not null && userRole != UserRole.Admin.ToString())
                return _mapper.Map<ICollection<GetClassRoomItemDTO>>(await getClasses
                                                .Where(c => c.StudentClasses.Any(sc => sc.Student.AppUserId == userId) 
                                                         || c.TeacherClasses.Any(tc=> tc.Teacher.AppUserId == userId))
                                                .Include(c=>c.StudentClasses)
                                                .ToListAsync());

            return _mapper.Map<ICollection<GetClassRoomItemDTO>>(await getClasses.ToListAsync());
        }
        public async Task<GetClassRoomDTO> GetByIdAsync(long id)
        {

            ClassRoom classRoom = await _roomRepository.GetByIdAsync(id,
                                                                    includes: [nameof(ClassRoom.StudentClasses)]);

            if (classRoom is null)
                throw new Exception("The Class Room isn't Found!");



            return _mapper.Map<GetClassRoomDTO>(classRoom);
        }
        public async Task CreateClassRoomAsync(PostClassRoomDTO postClass)
        {

            string userId = _currentUser.GetUserId();

            if (await _roomRepository.AnyAsync(r => r.Name == postClass.Name))
                throw new Exception("The Name has already used");

            ClassRoom newClass = new ClassRoom
            {
                Name = postClass.Name,
                PrivateKey = await _generateClassRoomKeyAsync(),
                TeacherClasses = new List<TeacherClassRoom>()
            };

            Teacher? teacher = await _teacherRepository.GetTeacherByUserIdAsync(userId);


            if (teacher is null)
            {
                teacher = new Teacher { AppUserId = userId };
                newClass.TeacherClasses.Add(new TeacherClassRoom { Teacher = teacher});         
            }
            else
            {
                newClass.TeacherClasses.Add(new TeacherClassRoom { TeacherId = teacher.Id});
            }

            _roomRepository.Add(newClass);
            
            await _roomRepository.SaveChangeAsync();
        }
        public async Task UpdateClassRoomAsync(long id, PutClassRoomDTO putClass)
        {

            if (await _roomRepository.AnyAsync(r => r.Name == putClass.Name))
                throw new Exception("The Name has already used");

            ClassRoom edited = await _roomRepository.GetByIdAsync(id);

            if (edited is null)
                throw new Exception("The ClassRoom isn't Found!");

            edited.Name = putClass.Name;

            _roomRepository.Update(edited);
            await _roomRepository.SaveChangeAsync();
        }
        public async Task DeleteClassRoomAsync(long id)
        {
            ClassRoom deleted = await _roomRepository.GetByIdAsync(id);

            if (deleted is null)
                throw new Exception("The ClassRoom isn't Found!");

            _roomRepository.Delete(deleted);
            await _roomRepository.SaveChangeAsync();
        }
        private async Task<string> _generateClassRoomKeyAsync()
        {
            char[] pool = "1234567890ABCDEFGHJKLMNPQRSTUVWXYZ".ToCharArray();
            string code;

            do
            {
                code = string.Empty;
                for (int i = 0; i < 8; i++)
                {
                    code += pool[Random.Shared.Next(pool.Length)];
                }
            }
            while (await _roomRepository.AnyAsync(r => r.PrivateKey == code));

            return code;
        }

   
    }
}
