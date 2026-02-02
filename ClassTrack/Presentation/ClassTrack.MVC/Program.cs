using ClassTrack.MVC.Middlewares;
using ClassTrack.MVC.Services.Implementations;
using ClassTrack.MVC.Services.Interfaces;

namespace ClassTrack.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IAuthenticationClientService, AuthenticationClientService>();
            builder.Services.AddScoped<ICookieService, CookieService>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<TokenHandlerMiddleware>();

            builder.Services.AddHttpClient("ClassTrackClient", config =>
            {
                config.BaseAddress = new Uri("https://localhost:7285/");
                config.DefaultRequestHeaders.Add("accept","application/json");
            });
        
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            app.Run();
        }
    }
}
