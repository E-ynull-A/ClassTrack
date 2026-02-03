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
    public class TeacherProfile:Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, GetTeacherClassItemDTO>()
                .ForCtorParam(nameof(GetTeacherClassItemDTO.TeacherClasses),opt=>opt.MapFrom(t=>t.TeacherClassRooms));
        }
    }
}
