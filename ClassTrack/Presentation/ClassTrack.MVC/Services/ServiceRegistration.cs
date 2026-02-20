using ClassTrack.MVC.Middlewares;
using ClassTrack.MVC.Services.Implementations;
using ClassTrack.MVC.Services.Interfaces;

namespace ClassTrack.MVC.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped<ICookieClientService, CookieClientService>();
            services.AddScoped<IAuthenticationClientService, AuthenticationClientService>();
            services.AddScoped<IClassRoomClientService, ClassRoomService>();
            services.AddScoped<ITokenClientService, TokenClientService>();
            services.AddScoped<IQuizClientService, QuizClientService>();
            services.AddScoped<IQuestionClientService, QuestionClientService>();
            services.AddScoped<IMemberClientService, MemberClientService>();
            services.AddScoped<IQuizAnswerClientService, QuizAnswerClientService>();
            services.AddScoped<IStudentAttendanceClientService, StudentAttendanceClientService>();
            services.AddScoped<ITaskWorkClientService, TaskWorkClientService>();
            services.AddScoped<IDashboardClientService, DashboardClientService>();


            services.AddHttpContextAccessor();
            services.AddTransient<TokenHandlerMiddleware>();

            return services;
        }
    }
}
