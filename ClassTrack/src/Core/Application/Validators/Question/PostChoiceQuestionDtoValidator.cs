using ClassTrack.Application.DTOs;
using FluentValidation;

namespace ClassTrack.Application.Validators
{
    internal class PostChoiceQuestionDtoValidator : AbstractValidator<PostChoiceQuestionDTO>
    {
        public PostChoiceQuestionDtoValidator()
        {
            RuleFor(q => q.Title)
                .NotEmpty()
                .MinimumLength(2);

            RuleFor(q => q.Point)
                .NotEmpty()
                .InclusiveBetween(1, 100);

            RuleFor(q => q.Options)
                .NotEmpty()
                .Must(o => o != null && o.Count >= 2)
                .WithMessage("There must be at least 2 Option!")

                .Must(o => o.Select(o => o.Variant).Distinct().Count() == o.Count);
                
                
                

            RuleFor(q => q)
                .Must(q => q.IsMultiple == false && q.Options
                      .Where(o => o.IsCorrect == true).Count() == 1)
                .WithMessage("You must choose only one correct Variant!");



                
          
        }
    }
}
