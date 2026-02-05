using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClassTrack.Infrastructure.Implementations.Services
{
    internal class TokenService: ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;
        private readonly ICacheService _cacheService;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration configuration,
                            IHttpContextAccessor accessor,
                            ICacheService cacheService,
                            UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _accessor = accessor;
            _cacheService = cacheService;
            _userManager = userManager;
        }
        public AccessTokenDTO CreateAccessToken(AppUser user,IEnumerable<string> roles,int minutes)
        {
            ICollection<Claim> claims = new List<Claim> {

                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.GivenName,user.Name),
                new Claim(ClaimTypes.Surname,user.Surname),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey 
                                                (Encoding.ASCII
                                                    .GetBytes(_configuration["JWT:securityKey"]));

            SigningCredentials credentials = new SigningCredentials
                                                    (key,SecurityAlgorithms.HmacSha256);


            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["JWT:issuer"],
                audience: _configuration["JWT:audience"],
                claims:claims,
                notBefore:DateTime.UtcNow,
                expires:DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials:credentials);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return new AccessTokenDTO(user.UserName,
                                token.ValidFrom,
                                handler.WriteToken(token));  
        }
        public RefreshTokenDTO GenerateRefreshToken()
        {              
            return new RefreshTokenDTO(Guid.NewGuid().ToString("N")
                  + Guid.NewGuid().ToString("N"));
        }
        public async Task<ResponseTokenDTO> RefreshAsync()
        {
            string? token = _accessor.HttpContext.Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("You have to login!");

            string? userId = await _cacheService.GetAsync<string>(token);

            if (string.IsNullOrEmpty(userId))
                throw new Exception("You have to login!");

            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("User not Found!");

            ICollection<string>? userRoles = await _userManager.GetRolesAsync(user);           

             await _cacheService.RemoveAsync(token);

            ResponseTokenDTO tokenDTO = new ResponseTokenDTO(CreateAccessToken(user, userRoles, 15),
                                                             GenerateRefreshToken());    

            await _cacheService.SetCasheAsync(tokenDTO.RefreshToken.RefreshToken,userId,TimeSpan.FromMinutes(60 * 24 * 7));
                

            return tokenDTO;         
        }
    }
}
