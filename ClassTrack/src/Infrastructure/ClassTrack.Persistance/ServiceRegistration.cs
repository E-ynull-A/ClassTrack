using ClassTrack.Persistance.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ClassTrack.Persistance
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServicePersistance(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("default")));

            return services;
        }
    }
}
