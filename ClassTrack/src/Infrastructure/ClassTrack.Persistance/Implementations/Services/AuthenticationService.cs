using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _manager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ICacheService _cacheService;
        private readonly IEmailService _emailService;

        public AuthenticationService(UserManager<AppUser> manager,
                                        IMapper mapper,
                                        ITokenService tokenService,
                                        ICacheService cacheService,
                                        IEmailService emailService)
        {
            _manager = manager;
            _mapper = mapper;
            _tokenService = tokenService;
            _cacheService = cacheService;
            _emailService = emailService;
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
            AppUser? user = null;

            if (loginDTO.UsernameOrEmail.Contains("@"))
            {
                user = await _manager.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.UsernameOrEmail);
            }

            else
            {
                user = await _manager.Users.FirstOrDefaultAsync(u => u.UserName == loginDTO.UsernameOrEmail);
            }
                
            if(user is null)
            {
                throw new Exception("User Couldn't Find!");
            }       

            IEnumerable<string> roles = await _manager.GetRolesAsync(user);
           
            ResponseTokenDTO response = new ResponseTokenDTO
            (
               _tokenService.CreateAccessToken(user, roles, 15),
               _tokenService.GenerateRefreshToken()
            );

            await _cacheService.SetCasheAsync(user.Id, response.RefreshToken, TimeSpan.FromDays(7));
            await _emailService.SendEmailAsync();


            return response;

        }
    }
}
