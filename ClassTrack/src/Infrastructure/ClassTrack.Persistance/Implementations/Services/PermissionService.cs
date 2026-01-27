using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Persistance.DAL;
using Microsoft.EntityFrameworkCore;




namespace ClassTrack.Persistance.Implementations.Services
{
    internal class PermissionService:IPermissionService
    {
        private readonly AppDbContext _context;
        private readonly ICasheService _casheService;

        public PermissionService(AppDbContext context,
                                 ICasheService casheService)
        {
            _context = context;
            _casheService = casheService;
        }
        public async Task<bool> IsTeacher(string userId,long classRoomId)
        {
            if(string.IsNullOrEmpty(userId) || classRoomId < 1)
                return false;

            string cacheKey = $"Is_Teacher_{userId}_{classRoomId}";

            return await _casheService.CheckCasheAsync(cacheKey, async () =>
            {
                return await _context.TeacherClasses.AnyAsync(tc => tc.Teacher.AppUserId == userId &&
                                                                tc.ClassRoomId == classRoomId);
            },TimeSpan.FromMinutes(10));
        }
    }
}
