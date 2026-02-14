using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
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
        private readonly IRefreshTokenRepository _tokenRepository;

        public AuthenticationService(UserManager<AppUser> manager,
                                        IMapper mapper,
                                        ITokenService tokenService,
                                        ICacheService cacheService,
                                        IEmailService emailService,
                                        SignInManager<AppUser> signInManager,
                                        IHttpContextAccessor accessor,
                                        IRefreshTokenRepository tokenRepository)
        {
            _manager = manager;
            _mapper = mapper;
            _tokenService = tokenService;
            _cacheService = cacheService;
            _emailService = emailService;
            _signInManager = signInManager;
            _accessor = accessor;
            _tokenRepository = tokenRepository;
        }
        public async Task RegisterAsync(RegisterDTO registerDTO)
        {
            AppUser user = _mapper.Map<AppUser>(registerDTO);

            IdentityResult result = await _manager.CreateAsync(user
                                                         , registerDTO.Password);

            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description;
                }
                throw new Exception(errors);
            }

            await _emailService.SendEmailAsync(user.Email, "Register Process", "Welcome to Our Application:)");

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


            ResponseTokenDTO response = new ResponseTokenDTO(aToken, rToken);


            _accessor.HttpContext.Response.Cookies.Delete("RefreshToken");
            _accessor.HttpContext.Response.Cookies.Delete("AccessToken");

            await _cacheService.SetCasheAsync(rToken.RefreshToken, user.Id, TimeSpan.FromDays(7));

            _tokenRepository.Add(new RefreshToken
            {
                ExpiryTime = DateTime.UtcNow.AddDays(7),
                Token = rToken.RefreshToken,
                UserId = user.Id
            });
            await _tokenRepository.SaveChangeAsync();

            return response;

        }
        public async Task LogoutAsync()
        {
            string? userId = _accessor.HttpContext
                                            .User
                                            .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not Found!");

            RefreshToken? rToken = await _tokenRepository
                                .FirstOrDefaultAsync(t => t.UserId == userId);

            if(rToken is not null)
            {
                _tokenRepository.Delete(rToken);                
            }

            await _cacheService.RemoveAsync(userId);    
            
            await _signInManager.SignOutAsync();
            await _tokenRepository.SaveChangeAsync();
        }
        public async Task ResetPasswordAsync(ResetPasswordDTO passwordDTO)
        {
            AppUser? user = await _manager.FindByEmailAsync(passwordDTO.Email);

            if (user == null)
                throw new Exception("User Not Found!");

            string? resetToken = await _cacheService.GetAsync<string>($"reset_Password:{passwordDTO.Email}");

            if (resetToken is null)
                throw new Exception("Please, try again");

            IdentityResult result = await _manager.ResetPasswordAsync(user, resetToken, passwordDTO.NewPassword);

            if (!result.Succeeded)
            {
                string excepts = string.Empty;
                foreach (IdentityError error in result.Errors)
                {
                    excepts = string.Concat(excepts, "\n", error.Description);
                }
                throw new Exception(excepts);
            }
        }
    }
}
