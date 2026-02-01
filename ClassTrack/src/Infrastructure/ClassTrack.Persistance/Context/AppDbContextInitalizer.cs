using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using ClassTrack.Persistance.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Context
{
    internal class AppDbContextInitalizer:IAppDbContextInitalizer
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AppDbContextInitalizer(UserManager<AppUser> userManager,
                                       RoleManager<IdentityRole> roleManager,
                                       IConfiguration config,
                                       AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _context = context;
        }

        public async Task InitalizeDbContext()
        {
            if(await _context.Database.EnsureCreatedAsync())
                    await _context.Database.MigrateAsync();
        }

        public async Task CreateRoleInitalizerAsync()
        {
            foreach(UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if(!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                   await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString()});
                }
            }
        }

        public async Task CreateAdminInitalizer()
        {
            if(!await _userManager.Users.AnyAsync(u=>u.UserName == _config["AdminData:Username"] || u.Email == _config["AdminData:Email"]))
            {
                AppUser appUser = new AppUser
                {
                    Name = "Eynulla",
                    Surname = "Mahmudov",
                    UserName = _config["AdminData:Username"],
                    Age = 19,
                    BirthDate = DateOnly.Parse("2006-10-10"),                   
                    Email = _config["AdminData:Email"],
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(appUser, _config["AdminData:Password"]);
                await _userManager.AddToRoleAsync(appUser, UserRole.Admin.ToString());
            }
        }
    }
}
