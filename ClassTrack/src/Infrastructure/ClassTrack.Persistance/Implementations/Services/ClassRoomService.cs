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
        private readonly IHttpContextAccessor _accessor;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IPermissionService _permissionService;

        public ClassRoomService(IClassRoomRepository roomRepository,
                                IMapper mapper,
                                IHttpContextAccessor accessor,
                                ITeacherRepository teacherRepository,
                                IPermissionService permissionService)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _accessor = accessor;
            _teacherRepository = teacherRepository;
            _permissionService = permissionService;
        }

        public async Task<ICollection<GetClassRoomItemDTO>> GetAllAsync(int page, int take)
        {
            string? userId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? userRole = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);           

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
            if (!await IsOwnAsync(id))
                throw new Exception("Forbidden activity Exception!");

            ClassRoom classRoom = await _roomRepository.GetByIdAsync(id,
                                                                    includes: [nameof(ClassRoom.StudentClasses)]);

            if (classRoom is null)
                throw new Exception("The Class Room isn't Found!");



            return _mapper.Map<GetClassRoomDTO>(classRoom);
        }
        public async Task CreateClassRoomAsync(PostClassRoomDTO postClass)
        {
            if (await _roomRepository.AnyAsync(r => r.Name == postClass.Name))
                throw new Exception("The Name has already used");

            string userId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new Exception("User isn't Found!");

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
            if (!(await _permissionService.IsTeacherAsync(id)).IsTeacher)
                throw new Exception("Forbidden actvity exception!");

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
            if (!(await _permissionService.IsTeacherAsync(id)).IsTeacher)
                throw new Exception("Forbidden actvity exception!");

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

        public async Task<bool> IsOwnAsync(long classRoomId)
        {
            string userId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new Exception("User isn't Found!");

            return await _roomRepository
                .AnyAsync(r =>r.Id == classRoomId && (r.StudentClasses
                        .Any(sc => sc.Student.AppUserId == userId) ||
                                 r.TeacherClasses.Any(tc => tc.Teacher.AppUserId == userId)));
        }
    }
}
