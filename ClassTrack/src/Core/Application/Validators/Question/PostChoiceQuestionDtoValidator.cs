using ClassTrack.Application.DTOs;
using FluentValidation;

namespace ClassTrack.Application.Validators
{
    public class PostChoiceQuestionDtoValidator : AbstractValidator<PostChoiceQuestionDTO>
    {
        public PostChoiceQuestionDtoValidator()
        {
            RuleFor(q => q.Title)
                .NotEmpty()
                .MinimumLength(2);

            RuleFor(q => q.Point)
                .NotNull()
                .Must(p => p >= 0 && p <= 100);

            RuleFor(q => q.Options)
                .NotEmpty()
                .Must(o => o.Count >= 2)
                .WithMessage("There must be at least 2 Option!")

                .Must(o => o.Select(o => o.Variant.Trim()).Distinct().Count() == o.Count);

            RuleFor(q => q)
                .Must(q =>
                {
                    if (q.Options is null)
                        return true;

                    else if (q.IsMultiple)
                    {
                        return q.Options.Count(o => o.IsCorrect) > 0;
                    }
                    else
                    {
                        return q.Options.Count(o => o.IsCorrect) == 1;
                    }

                })
                .WithMessage(q => q.IsMultiple
                ? "You must choose only one correct Variant!"
                : "You must choose at Least one correct Variant!");
               



                
          
        }
    }
}
