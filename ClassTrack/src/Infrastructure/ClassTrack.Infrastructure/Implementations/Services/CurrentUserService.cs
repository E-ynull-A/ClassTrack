using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Utilities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;




namespace ClassTrack.Infrastructure.Implementations.Services
{
    internal class CurrentUserService:ICurrentUserService
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string GetUserId()
        {
            string? userId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                throw new UnauthorizedAccessException();

            return userId;
        }

        public string GetUserRole()
        {
            string? roles = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

            if (roles is null)
                throw new NotFoundException("The User Role not Found!");

            return roles;
        }
    }
}
