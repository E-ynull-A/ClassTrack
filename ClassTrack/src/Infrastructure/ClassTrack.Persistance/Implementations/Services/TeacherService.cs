using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Persistance.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class TeacherService:ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _context;

        public TeacherService(ITeacherRepository teacherRepository,
                                IMemoryCache cache,
                                AppDbContext context)
        {
            _teacherRepository = teacherRepository;
            _cache = cache;
            _context = context;
        }

        //public async Task<bool> IsTeacher(string userId,long classRoomId)
        //{
        //    string cacheKey = $"is Teacher_{userId}_{classRoomId}";

        //    if (_cache.TryGetValue(cacheKey, out bool isTeacher))
        //    {
        //        isTeacher = await _context.TeacherClasses.AnyAsync(tc => tc.Teacher.AppUserId == userId && tc.ClassRoomId == classRoomId);

        //        _cache.Set(cacheKey, isTeacher, TimeSpan.FromMinutes(10));
        //    }
            

        //    return isTeacher;
        //}
    }
}
