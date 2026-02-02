using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Infrastructure.Implementations.Services
{
    internal class TokenService: ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
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
            return new RefreshTokenDTO(new Guid().ToString("N")
                  + new Guid().ToString("N"));
        }
    }
}
