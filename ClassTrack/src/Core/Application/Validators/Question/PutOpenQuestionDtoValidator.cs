using ClassTrack.Application.DTOs;
using FluentValidation;

namespace ClassTrack.Application.Validators
{
    public class PutOpenQuestionDtoValidator:AbstractValidator<PutOpenQuestionDTO>
    {
        public PutOpenQuestionDtoValidator()
        {
            RuleFor(q => q.Title)
                .NotEmpty()
                .MinimumLength(2);

            RuleFor(q => q.Point)
                .NotNull()
                .InclusiveBetween(0,100);
        }
    }
}
