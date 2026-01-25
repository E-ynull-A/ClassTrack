using ClassTrack.Application.DTOs;
using ClassTrack.Domain.Utilities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class PostQuizDtoValidator:AbstractValidator<PostQuizDTO>
    {
        public PostQuizDtoValidator()
        {
            RuleFor(q => q.Name)
                .NotEmpty()
                .MaximumLength(85);

            RuleFor(q => q.StartTime)
                .NotEmpty()
                .Must(st => st >= DateTime.UtcNow)
                .WithMessage("The Start Time of Quiz must be in the future or present");

            RuleFor(q => q.Duration)
                .NotEmpty()
                .LessThanOrEqualTo(TimeSpan.FromHours(24))
                .GreaterThan(TimeSpan.FromHours(0));

            RuleFor(q => q)
                .NotEmpty()
                .Must(qq => qq.ChoiceQuestions.Count + qq.OpenQuestions.Count <= 200)
                .WithMessage("The Question Count isn't exceed the limit");

            RuleFor(q => q.ClassRoomId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
