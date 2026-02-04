using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Persistance.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;

        public TeacherService(ITeacherRepository teacherRepository,
                              IMapper mapper,
                              IHttpContextAccessor accessor)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
            _accessor = accessor;
        }

        //public async Task<GetTeacherClassItemDTO> GetTeacherClassesAsync()
        //{
        //    string userId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (string.IsNullOrEmpty(userId))
        //        throw new Exception("User not Found!");

        //    Teacher teacher = await _teacherRepository.FirstOrDefaultAsync(t => t.AppUserId == userId,includes ["TeacherClassRooms.ClassRoom"]);

        //    if (teacher is null)
        //        throw new Exception("The Teacher not Found!");

        //    return _mapper.Map<GetTeacherClassItemDTO>(teacher);
        //}

    }
}
