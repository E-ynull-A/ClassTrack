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
    public class PutStudentAttendanceDtoValidator : AbstractValidator<PutStudentAttendanceDTO>
    {
        public PutStudentAttendanceDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);


            RuleFor(sa => sa.ClassRoomId)
                 .NotEmpty()
                 .GreaterThan(0);


            RuleFor(sa => sa.StudentId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(sa => sa.Attendance)
                .Must(a =>
                {
                    foreach (Attendance attend in Enum.GetValues(typeof(Attendance)))//--><><!><><--\\
                    {
                        if (attend == (Attendance)a)
                        {
                            return true;
                        }
                    }
                    return false;
                })
                .WithMessage("There is an issue about Attendance Status!");

            RuleFor(sa => sa.LessonDate)
                .NotEmpty()
                .Must(ld => ld <= DateTime.UtcNow)
                .WithMessage("The Attendance of the Lesson Date couldn't created before the lesson begun!");
        }
    }
}
