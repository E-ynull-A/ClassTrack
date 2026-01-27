using ClassTrack.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class PutTaskWorkDtoValidator:AbstractValidator<PutTaskWorkDTO>
    {
        public PutTaskWorkDtoValidator()
        {
            RuleFor(tw => tw.ClassRoomId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(tw => tw.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(tw => tw.StartDate)
                .NotEmpty()
                .Must(sd => sd >= DateTime.UtcNow)
                .WithMessage("Task doesn't begin in past");

            RuleFor(tw => tw)
                .NotEmpty()
                .Must(tw => tw.EndDate > tw.StartDate)
                .WithMessage("The Task must ended after the Start Time")
                .Must(tw=>tw.MainPart.Trim() != tw.Title.Trim())
                .WithMessage("The Title and The Main part is Different things");
           
            RuleFor(tw => tw.MainPart)
                .NotEmpty();
        }
    }
}
