using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using ClassTrack.Domain.Utilities;
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
        private readonly ICurrentUserService _currentUserService;
        private readonly IRefreshTokenRepository _tokenRepository;

        public AuthenticationService(UserManager<AppUser> manager,
                                        IMapper mapper,
                                        ITokenService tokenService,
                                        ICacheService cacheService,
                                        IEmailService emailService,
                                        SignInManager<AppUser> signInManager,
                                        ICurrentUserService currentUserService,
                                        IRefreshTokenRepository tokenRepository)
        {
            _manager = manager;
            _mapper = mapper;
            _tokenService = tokenService;
            _cacheService = cacheService;
            _emailService = emailService;
            _signInManager = signInManager;
            _currentUserService = currentUserService;
            _tokenRepository = tokenRepository;
        }
        public async Task RegisterAsync(RegisterDTO registerDTO)
        {
            AppUser user = _mapper.Map<AppUser>(registerDTO);

            IdentityResult result = await _manager.CreateAsync(user,registerDTO.Password);

            await _emailService
                .SendEmailAsync(user.Email, "Register Process"
                                    ,"Welcome to Our Application:)");

            await _manager.AddToRoleAsync(user, UserRole.User.ToString());
        }
        public async Task<ResponseTokenDTO> LoginAsync(LoginDTO loginDTO)
        {
            AppUser? user = await _manager.Users.FirstOrDefaultAsync(u => u.UserName == loginDTO.UsernameOrEmail
                                                                       || u.Email == loginDTO.UsernameOrEmail);

            if (user == null)
            {
                throw new NotFoundException("The Username,Email or Password is Wrong!");
            }
        
            if(await _manager.IsLockedOutAsync(user))
            {
                TimeSpan minutes = TimeSpan.FromMinutes(Math.Ceiling((user.LockoutEnd.Value - DateTime.UtcNow).TotalMinutes));
                if(minutes.TotalMinutes > 10)
                {
                    throw new ConflictException($"You Banned and can enter {((int)minutes.TotalDays >= 1?$"{(int)minutes.TotalDays} Day later"
                                                                                           : ((int)minutes.TotalHours >= 1)? $"{(int)minutes.TotalHours} Hour later" 
                                                                                           : $"{(int)minutes.TotalMinutes} Minutes later")}");
                }

                throw new ConflictException($"Try again {minutes} minutes later...");
            }

            bool check = await _manager.CheckPasswordAsync(user, loginDTO.Password);     

            if (!check)
            {
                await _manager.AccessFailedAsync(user);
                throw new NotFoundException("The Username,Email or Password is Wrong!");
            }

            IEnumerable<string> roles = await _manager.GetRolesAsync(user);

            RefreshTokenDTO rToken = _tokenService.GenerateRefreshToken();
            AccessTokenDTO aToken = _tokenService.CreateAccessToken(user, roles, 15);


            ResponseTokenDTO response = new ResponseTokenDTO(aToken, rToken);


            _currentUserService.DeleteCookie("RefreshToken");
            _currentUserService.DeleteCookie("AccessToken");

            RefreshToken? oldToken = await _tokenRepository.FirstOrDefaultAsync(t => t.UserId == user.Id);

            if(oldToken != null)
            {
                _tokenRepository.Delete(oldToken);
            }

            await _cacheService.SetCasheAsync(rToken.RefreshToken, user.Id, TimeSpan.FromMinutes(15*4));

            _tokenRepository.Add(new RefreshToken
            {
                ExpiryTime = DateTime.UtcNow.AddMinutes(60),
                Token = rToken.RefreshToken,
                UserId = user.Id
            });
            await _tokenRepository.SaveChangeAsync();

            return response;

        }
        public async Task LogoutAsync()
        {
            string? userId = _currentUserService.GetUserId();

            if (string.IsNullOrEmpty(userId))
                throw new NotFoundException("User not Found!");

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
                throw new NotFoundException("User Not Found!");

            string? resetToken = await _cacheService.GetAsync<string>($"reset_Password:{passwordDTO.Email}");

            if (resetToken is null)
                throw new NotFoundException("Please, try again");

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
        public async Task BanUserAsync(PostBanUserDTO postBan)
        {
            AppUser? user = await _manager.FindByIdAsync(postBan.UserId);

            if (user is null)
                throw new NotFoundException("User not Found!");

            DateTimeOffset date = postBan.Unit == "Day" ? DateTimeOffset.UtcNow.AddDays(postBan.Duration) :
                                   postBan.Unit == "Hour" ? DateTimeOffset.UtcNow.AddHours(postBan.Duration) :
                                   throw new BadRequestException();

            Console.WriteLine($"Server UTC Vaxtı: {DateTimeOffset.UtcNow}");
            Console.WriteLine($"Təyin olunan Ban Vaxtı: {date}");

            await _manager.SetLockoutEnabledAsync(user,true);
            await _manager.SetLockoutEndDateAsync(user,date);


            await _manager.UpdateSecurityStampAsync(user);      
        }
    }
}
