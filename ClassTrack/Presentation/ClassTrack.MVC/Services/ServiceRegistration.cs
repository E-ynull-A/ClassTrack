using ClassTrack.MVC.Middlewares;
using ClassTrack.MVC.Services.Implementations;
using ClassTrack.MVC.Services.Interfaces;

namespace ClassTrack.MVC.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IAuthenticationClientService, AuthenticationClientService>();
            services.AddScoped<IClassRoomService, ClassRoomService>();
            services.AddScoped<ITokenClientService, TokenClientService>();


            services.AddHttpContextAccessor();
            services.AddTransient<TokenHandlerMiddleware>();

            return services;
        }
    }
}
