using ClassTrack.Application.DTOs;
using FluentValidation;

namespace ClassTrack.Application.Validators
{
    internal class PostOpenQuestionDtoValidator:AbstractValidator<PostOpenQuestionDTO>
    {
        public PostOpenQuestionDtoValidator()
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
