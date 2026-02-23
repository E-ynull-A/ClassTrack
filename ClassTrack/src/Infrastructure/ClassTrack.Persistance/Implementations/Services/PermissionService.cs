using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using ClassTrack.Domain.Utilities;
using ClassTrack.Persistance.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;




namespace ClassTrack.Persistance.Implementations.Services
{
    internal class PermissionService:IPermissionService
    {
        private readonly ICacheService _casheService;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<AppUser> _userManager;

        public PermissionService(ICacheService casheService,
                                 ITeacherRepository teacherRepository,
                                 ICurrentUserService currentUser,
                                 UserManager<AppUser> userManager)
        {
            _casheService = casheService;
            _teacherRepository = teacherRepository;
            _currentUser = currentUser;
            _userManager = userManager;
        }
        public async Task<IsTeacherDTO> IsTeacherAsync(long classRoomId)
        {
            string userId = _currentUser.GetUserId();


            if (string.IsNullOrEmpty(userId) || classRoomId < 1)
                throw new BadRequestException("Invalid User and ClassRoom IDs!!");

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

        public async Task<bool> IsAdminAsync(string emailOrUserName)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == emailOrUserName
                                                                         || u.Email == emailOrUserName);

            if (user == null)
                throw new NotFoundException("User not Found");

            return await _userManager.IsInRoleAsync(user, UserRole.Admin.ToString());
        }
    }
}
