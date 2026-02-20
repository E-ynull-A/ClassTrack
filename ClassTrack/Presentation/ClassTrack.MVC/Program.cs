using ClassTrack.MVC.Middlewares;
using ClassTrack.MVC.Services.Implementations;
using ClassTrack.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using ClassTrack.MVC.Services;

namespace ClassTrack.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddService();

            builder.Services.AddHttpClient("ClassTrackClient", config =>
            {
                config.BaseAddress = new Uri("https://localhost:7285/");
                config.DefaultRequestHeaders.Add("accept","application/json");
            }).AddHttpMessageHandler<TokenHandlerMiddleware>();

            builder.Services.AddHttpClient("RefreshTokenClient", client => {
                client.BaseAddress = new Uri("https://localhost:7285/");
            });


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.LoginPath = "/Home/Login";
                });
            

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
                pattern: "{area:exists}/{controller=home}/{action=index}/{id?}");

            app.MapControllerRoute(
                name: "area",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
