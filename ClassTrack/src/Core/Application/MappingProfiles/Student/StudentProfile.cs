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
    internal class StudentProfile:Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, GetStudentItemDTO>()
                .ForCtorParam(nameof(GetStudentItemDTO.Name),opt=>opt.MapFrom(s=>s.AppUser.Name))
                .ForCtorParam(nameof(GetStudentItemDTO.Surname), opt => opt.MapFrom(s => s.AppUser.Surname))
                .ForCtorParam(nameof(GetStudentItemDTO.JoinedAt),opt=>opt.MapFrom(s=>s.StudentClasses
                                                                         .Select(sc=>sc.JoinedAt).FirstOrDefault()));
                
        }
    }
}
