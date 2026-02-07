using ClassTrack.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class PutQuizDtoValidator:AbstractValidator<PutQuizDTO>
    {
        public PutQuizDtoValidator()
        {
            RuleFor(q => q.Name)
                .NotEmpty()
                .MaximumLength(85);

            RuleFor(q => q.StartTime)
                .NotEmpty()
                .Must(st => st >= DateTime.UtcNow)
                .WithMessage("The Start Time of Quiz must be in the future or present");

            RuleFor(q => q.Duration)
                .LessThanOrEqualTo(60*24)
                .GreaterThan(0);

            RuleFor(q => q.ClassRoomId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
