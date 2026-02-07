using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
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
        private readonly ICacheService _casheService;
        private readonly IHttpContextAccessor _accessor;
        private readonly ITeacherRepository _teacherRepository;

        public PermissionService(ICacheService casheService,
                                 IHttpContextAccessor accessor,
                                 ITeacherRepository teacherRepository)
        {
            _casheService = casheService;
            _accessor = accessor;
            _teacherRepository = teacherRepository;
        }
        public async Task<IsTeacherDTO> IsTeacherAsync(long classRoomId)
        {
            string? userId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
 

            if (string.IsNullOrEmpty(userId) || classRoomId < 1)
                throw new Exception("Invalid User and ClassRoom IDs!!");

            string cacheKey = $"Is_Teacher_{userId}_{classRoomId}";

            var result = await _casheService.GetAsync<bool?>(cacheKey);

            if(result.HasValue)
                   return new IsTeacherDTO(result.Value);

            IsTeacherDTO tDTO = new IsTeacherDTO(await _teacherRepository.AnyAsync(t => t.AppUserId == userId && t.TeacherClassRooms
                                                                     .Any(tc => tc.ClassRoomId == classRoomId)));

            await _casheService
                        .SetCasheAsync(cacheKey, tDTO.IsTeacher, TimeSpan.FromMinutes(10));

            return tDTO;
        }      
    }
}
