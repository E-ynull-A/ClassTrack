using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _manager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthenticationService(UserManager<AppUser> manager,
                                        IMapper mapper,
                                        ITokenService tokenService)
        {
            _manager = manager;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        public async Task RegisterAsync(RegisterDTO registerDTO)
        {
            IdentityResult result = await _manager.CreateAsync(_mapper
                                                     .Map<AppUser>(registerDTO)
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
        }


        public async Task<TokenDTO> LoginAsync(LoginDTO loginDTO)
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

             return _tokenService.CreateAccessToken(user);
        }
    }
}
