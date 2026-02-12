using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _manager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ICacheService _cacheService;
        private readonly IEmailService _emailService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _accessor;

        public AuthenticationService(UserManager<AppUser> manager,
                                        IMapper mapper,
                                        ITokenService tokenService,
                                        ICacheService cacheService,
                                        IEmailService emailService,
                                        SignInManager<AppUser> signInManager,
                                        IHttpContextAccessor accessor)
        {
            _manager = manager;
            _mapper = mapper;
            _tokenService = tokenService;
            _cacheService = cacheService;
            _emailService = emailService;
            _signInManager = signInManager;
            _accessor = accessor;
        }
        public async Task RegisterAsync(RegisterDTO registerDTO)
        {
            AppUser user = _mapper.Map<AppUser>(registerDTO);

            IdentityResult result = await _manager.CreateAsync(user
                                                         ,registerDTO.Password);

            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description;
                }
                throw new Exception(errors);
            }

            

            await _manager.AddToRoleAsync(user, UserRole.User.ToString());
        }
        public async Task<ResponseTokenDTO> LoginAsync(LoginDTO loginDTO)
        {
            AppUser? user = await _manager.Users.FirstOrDefaultAsync(u => u.UserName == loginDTO.UsernameOrEmail 
                                                                       || u.Email == loginDTO.UsernameOrEmail);

            if (user == null)
            {
                throw new Exception("The Username,Email or Password is invalid!");
            }

            bool check = await _manager.CheckPasswordAsync(user, loginDTO.Password);

            if (!check)
            {
                await _manager.AccessFailedAsync(user);
                throw new Exception("The Username,Email or Password is invalid!");
            }

            IEnumerable<string> roles = await _manager.GetRolesAsync(user);

            RefreshTokenDTO rToken = _tokenService.GenerateRefreshToken();
            AccessTokenDTO aToken = _tokenService.CreateAccessToken(user, roles, 15);


            ResponseTokenDTO response = new ResponseTokenDTO(aToken,rToken);


            _accessor.HttpContext.Response.Cookies.Delete("RefreshToken");
            _accessor.HttpContext.Response.Cookies.Delete("AccessToken");

            await _cacheService.SetCasheAsync(response.RefreshToken.RefreshToken, user.Id, TimeSpan.FromDays(7));
            await _emailService.SendEmailAsync();          

            return response;

        }
        public async Task LogoutAsync()
        {
            string? userId = _accessor.HttpContext
                                            .User
                                            .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not Found!");

           await _cacheService.RemoveAsync(userId);

           await _signInManager.SignOutAsync();
        }
        public void ResetPasswordAsync(ResetPasswordDTO passwordDTO)
        {
            
        }
    }
}
