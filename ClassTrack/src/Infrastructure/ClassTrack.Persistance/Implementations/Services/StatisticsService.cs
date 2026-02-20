using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Enums;
using ClassTrack.Persistance.Implementations.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class StatisticsService : IStatisticsService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly ITaskWorkRepository _taskWorkRepository;
        private readonly UserManager<AppUser> _userManager;

        public StatisticsService(IStudentRepository studentRepository,
                                 ITeacherRepository teacherRepository,
                                 IQuizRepository quizRepository,
                                 ITaskWorkRepository taskWorkRepository,
                                 UserManager<AppUser> userManager)
        {
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _quizRepository = quizRepository;
            _taskWorkRepository = taskWorkRepository;
            _userManager = userManager;
        }

        public async Task<GetStatisticsDTO> GetCountAsync()
        {
            return new GetStatisticsDTO(
                await _studentRepository.CountAsync(),
                await _teacherRepository.CountAsync(),
                await _quizRepository.CountAsync(),
                await _taskWorkRepository.CountAsync());
        }

        public async Task<GetUserPagedItemDTO> GetAllUserAsync(int page, int take)
        {
            ICollection<AppUser> users = _userManager.Users
                                            .Include(u => u.Student)
                                            .Include(u => u.Teacher)
                                            .Skip((page-1)*take)
                                            .Take(take)
                                            .ToImmutableList();

            IList<string> roles = (await _userManager.GetUsersInRoleAsync(UserRole.Admin.ToString()))
                                    .Select(u => u.Id).ToImmutableList();


            ICollection<GetUserItemDTO> userInfos = users.Select(u => new GetUserItemDTO
            (
                u.Id,
                u.Email,
                string.Concat(u.Name, " ", u.Surname),
                new Collection<string>
                {
                     u.Student != null ? "Student" : null,
                     u.Teacher != null ? "Teacher" : null,
                     roles.Contains(u.Id) ? "Admin" : null
                }.Where(r=>r is not null).ToImmutableList())
            ).ToImmutableList(); 
            
           
            
            return new GetUserPagedItemDTO(userInfos,_userManager.Users.Count());
        }
    }
}
