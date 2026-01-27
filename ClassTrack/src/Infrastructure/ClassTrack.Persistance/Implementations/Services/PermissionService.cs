using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Persistance.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;




namespace ClassTrack.Persistance.Implementations.Services
{
    internal class PermissionService:IPermissionService
    {
        private readonly AppDbContext _context;
        private readonly ICasheService _casheService;
        private readonly IHttpContextAccessor _accessor;

        public PermissionService(AppDbContext context,
                                 ICasheService casheService,
                                 IHttpContextAccessor accessor)
        {
            _context = context;
            _casheService = casheService;
            _accessor = accessor;
        }
        public async Task<bool> IsTeacherAsync(long classRoomId)
        {
            string userId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new Exception("The User is not Found!");

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
