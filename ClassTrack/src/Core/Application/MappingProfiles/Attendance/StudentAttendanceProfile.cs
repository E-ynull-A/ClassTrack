using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.MappingProfiles.Attendance
{
    internal class StudentAttendanceProfile:Profile
    {
        public StudentAttendanceProfile()
        {
            CreateMap<PostStudentAttendanceDTO, StudentAttendance>();
            CreateMap<StudentAttendance, GetStudentAttendanceItemDTO>()
                .ForCtorParam(nameof(GetStudentAttendanceItemDTO.StudentName),opt=>opt.MapFrom(s=>s.Student.AppUser.Name))
                .ForCtorParam(nameof(GetStudentAttendanceItemDTO.StudentSurname),opt=>opt.MapFrom(s=>s.Student.AppUser.Surname));

            CreateMap<PutStudentAttendanceDTO, StudentAttendance>();
        }
    }
}
