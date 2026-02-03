using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Persistance.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public TeacherService(ITeacherRepository teacherRepository,
                              IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
        }

        public async Task<GetTeacherClassItemDTO> GetTeacherClassesAsync(string userId)
        {
            Teacher teacher = await _teacherRepository.FirstOrDefaultAsync(t => t.AppUserId == userId, ["TeacherClasses"]);

            if (teacher is null)
                throw new Exception("The Teacher isn't Found");

            return _mapper.Map<GetTeacherClassItemDTO>(teacher);
        }

    }
}
