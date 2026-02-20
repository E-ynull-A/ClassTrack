using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Domain.Entities;

namespace ClassTrack.Application.MappingProfiles
{
    public class ClassRoomProfile:Profile
    {
        public ClassRoomProfile()
        {
            CreateMap<ClassRoom, GetClassRoomItemDTO>()
                .ForCtorParam(nameof(GetClassRoomItemDTO.StudentCount),
                              opt => opt.MapFrom(c => c.StudentClasses.Count))
                .ForCtorParam(nameof(GetClassRoomItemDTO.TeacherFullNames),
                              opt=>opt.MapFrom(tn=>tn.TeacherClasses
                                        .Select(tc=>tc.Teacher.AppUser.Name + tc.Teacher.AppUser.Surname)));
       

            CreateMap<ClassRoom, GetClassRoomDTO>()
                .ForCtorParam(nameof(GetClassRoomDTO.MemberCount),
                              opt=>opt.MapFrom(c=>c.StudentClasses.Count));

        }
    }
}
