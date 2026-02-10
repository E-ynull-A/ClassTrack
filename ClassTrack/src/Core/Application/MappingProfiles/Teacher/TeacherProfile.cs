using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Domain.Entities;

namespace ClassTrack.Application.MappingProfiles
{
    public class TeacherProfile:Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, GetTeacherItemDTO>()
                .ForCtorParam(nameof(GetTeacherItemDTO.Name), opt => opt.MapFrom(t => t.AppUser.Name))
                .ForCtorParam(nameof(GetTeacherItemDTO.Surname), opt => opt.MapFrom(t => t.AppUser.Surname));
        }
    }
}
