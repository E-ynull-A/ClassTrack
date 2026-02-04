using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.MappingProfiles
{
    public class ClassRoomProfile:Profile
    {
        public ClassRoomProfile()
        {
            CreateMap<ClassRoom, GetClassRoomItemDTO>()
                .ForCtorParam(nameof(GetClassRoomItemDTO.MemberCount),
                              opt => opt.MapFrom(c => c.StudentClasses.Count));
       

            CreateMap<ClassRoom, GetClassRoomDTO>()
                .ForCtorParam(nameof(GetClassRoomDTO.MemberCount),
                              opt=>opt.MapFrom(c=>c.StudentClasses.Count));

        }
    }
}
