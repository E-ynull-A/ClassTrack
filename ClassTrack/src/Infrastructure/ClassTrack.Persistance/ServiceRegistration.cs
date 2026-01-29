using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Persistance.DAL;
using ClassTrack.Persistance.Implementations.Repositories;
using ClassTrack.Persistance.Implementations.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ClassTrack.Persistance
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServicePersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("default")));

            services.AddIdentity<AppUser, IdentityRole>
                (
                    opt =>
                    {
                        opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._+";
                        opt.User.RequireUniqueEmail = true;

                        opt.Password.RequiredUniqueChars = 2;
                        opt.Password.RequiredLength = 8;
                        opt.Password.RequireNonAlphanumeric = false;

                        opt.Lockout.MaxFailedAccessAttempts = 5;
                        opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);                                                       
                    }

                )
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDbContext>();


            services.AddScoped<IClassRoomRepository, ClassRoomRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IStudentAttendanceRepository, StudentAttendanceRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITaskWorkRepository, TaskWorkRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IOptionRepository, OptionRepository>();
            services.AddScoped<IQuizAnswerRepository, QuizAnswerRepository>();
            services.AddScoped<IStudentQuizRepository, StudentQuizRepository>();



            services.AddScoped<IClassRoomService, ClassRoomService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IStudentAttendanceService, StudentAttendanceService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITaskWorkService, TaskWorkService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IQuizAnswerService, QuizAnswerService>();
            services.AddScoped<IStudentQuizService, StudentQuizService>();

            return services;
        }
    }
}
