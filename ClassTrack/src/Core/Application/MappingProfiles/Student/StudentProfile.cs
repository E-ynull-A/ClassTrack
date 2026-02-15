using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Domain.Entities;


namespace ClassTrack.Application.MappingProfiles
{
    internal class StudentProfile:Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, GetStudentItemDTO>()
                .ForCtorParam(nameof(GetStudentItemDTO.Name),opt=>opt.MapFrom(s=>s.AppUser.Name))
                .ForCtorParam(nameof(GetStudentItemDTO.Surname), opt => opt.MapFrom(s => s.AppUser.Surname))
                .ForCtorParam(nameof(GetStudentItemDTO.JoinedAt),opt=>opt.MapFrom(s=>s.StudentClasses
                                                                         .Select(sc=>sc.JoinedAt).FirstOrDefault()));
            
            CreateMap<Student, GetSimpleStudentItemDTO>()
                .ForCtorParam(nameof(GetSimpleStudentItemDTO.Name), opt => opt.MapFrom(s => s.AppUser.Name))
                .ForCtorParam(nameof(GetSimpleStudentItemDTO.Surname), opt => opt.MapFrom(s => s.AppUser.Surname));
        }
    }
}
