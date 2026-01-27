using ClassTrack.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class PutOptionInChoiceQuestionDtoValidator:AbstractValidator<PutOptionInChoiceQuestionDTO>
    {
        private const int MAX_LENGTH = 256;
        public PutOptionInChoiceQuestionDtoValidator()
        {
             RuleFor(o => o.Variant)
            .NotEmpty()
            .MaximumLength(MAX_LENGTH);

             RuleFor(o => o.IsCorrect)
                .NotEmpty();
        }
    }
}
