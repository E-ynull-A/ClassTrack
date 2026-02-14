using ClassTrack.Application.DTOs;
using ClassTrack.Application.DTOs.Token;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;
        private readonly ICacheService _cacheService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IRefreshTokenRepository _tokenRepository;

        public TokenService(IConfiguration configuration,
                            IHttpContextAccessor accessor,
                            ICacheService cacheService,
                            UserManager<AppUser> userManager,
                            IEmailService emailService,
                            IRefreshTokenRepository tokenRepository)
        {
            _configuration = configuration;
            _accessor = accessor;
            _cacheService = cacheService;
            _userManager = userManager;
            _emailService = emailService;
            _tokenRepository = tokenRepository;
        }
        public AccessTokenDTO CreateAccessToken(AppUser user, IEnumerable<string> roles, int minutes)
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
                                                    (key, SecurityAlgorithms.HmacSha256);


            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["JWT:issuer"],
                audience: _configuration["JWT:audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: credentials);

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
        public async Task<ResponseTokenDTO> RefreshAsync(string rToken)
        {
            if (string.IsNullOrEmpty(rToken))
                throw new Exception("You have to login!");

            string? userId = await _cacheService.GetAsync<string>(rToken);         

            if (string.IsNullOrEmpty(userId))
            {
                var token = await _tokenRepository.FirstOrDefaultAsync(t => t.Token == rToken);
                if(token is null)
                    throw new Exception("You have to login!");
                userId = token.UserId;
            }
               
            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("User not Found!");

            ICollection<string>? userRoles = await _userManager.GetRolesAsync(user);

            await _cacheService.RemoveAsync(rToken);

            ResponseTokenDTO tokenDTO = new ResponseTokenDTO(CreateAccessToken(user, userRoles, 15),
                                                             GenerateRefreshToken());

            await _cacheService.SetCasheAsync(tokenDTO.RefreshToken.RefreshToken, userId, TimeSpan.FromMinutes(60 * 24 * 7));
            _tokenRepository.Add(new RefreshToken
            {
                ExpiryTime = DateTime.UtcNow.AddDays(7),
                UserId = userId,
                Token = tokenDTO.RefreshToken.RefreshToken,                
            });
            await _tokenRepository.SaveChangeAsync();

            return tokenDTO;
        }
        public async Task GenerateResetTokenAsync(ResetTokenDTO passwordDTO)
        {
            string casheKey = $"reset_Password:{passwordDTO.Email}";

            AppUser? user = await _userManager.FindByEmailAsync(passwordDTO.Email);

            if (user == null)
                throw new Exception("The User Not Found");

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _cacheService.SetCasheAsync(casheKey,
                                              token,
                                              TimeSpan.FromMinutes(10));

            
            string encodedEmail = HttpUtility.UrlEncode(passwordDTO.Email);

            await _emailService.SendEmailAsync(passwordDTO.Email,
                                               "Password Reset ClassTrack Account",
                                               $"{_configuration["MVC:Url"]}Home/Reset?email={encodedEmail}");                               
        }

    }
}
