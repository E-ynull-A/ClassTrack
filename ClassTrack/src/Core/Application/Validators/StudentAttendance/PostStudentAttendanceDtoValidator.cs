using ClassTrack.Application.DTOs;
using ClassTrack.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class PostStudentAttendanceDtoValidator:AbstractValidator<PostStudentAttendanceDTO>
    {
        public PostStudentAttendanceDtoValidator()
        {
            RuleFor(sa => sa.ClassRoomId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(sa => sa.StudentId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(sa => sa.Attendance)
                .Must(a =>
                {
                    foreach (Attendance attend in Enum.GetValues(typeof(Attendance)))
                    {
                        if (attend == (Attendance)a)
                        {
                            return true;
                        }
                    }
                    return false;
                })
                .WithMessage("There is an issue about Attendance Status!");
        }
    }
}
