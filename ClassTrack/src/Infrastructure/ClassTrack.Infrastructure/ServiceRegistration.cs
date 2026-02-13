using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Infrastructure.Implementations.Services;
using ClassTrack.Persistance.Implementations.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Infrastructure
{
    public static class ServiceRegistration
    {  
        public static IServiceCollection InfrastructureRegistration(this IServiceCollection services,IConfiguration config)
        {         
            services.AddScoped<ITokenService, TokenService>();
         
            
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt => opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = config["JWT:issuer"],
                ValidAudience = config["JWT:audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["JWT:securityKey"])),
                LifetimeValidator = (_, exp, token, _) =>  exp is not null && token is not null ? DateTime.UtcNow < exp : false ,

                ClockSkew = TimeSpan.Zero
            });

            services.AddStackExchangeRedisCache(opt =>
            {
                opt.InstanceName = config["RedisCache:Name"];
                opt.Configuration = config["RedisCache:Configuration"];
            });


            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
      

            return services;
            
        }
    }
}
