using ClassTrack.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class PutQuizAnswerDtoValidator:AbstractValidator<PutQuizAnswerDTO>
    {
        public PutQuizAnswerDtoValidator()
        {
            RuleFor(qa => qa.Point)
                 .NotEmpty()
                 .InclusiveBetween(0,100);

        }
    }
}
