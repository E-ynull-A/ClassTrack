using ClassTrack.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class PutQuizSubmitDtoValidator:AbstractValidator<PutQuizSubmitDTO>
    {
        public PutQuizSubmitDtoValidator()
        {
            RuleFor(qs => qs.Point)
                .InclusiveBetween(0, 100);

            RuleFor(qs => qs.QuizAnswerId)
                .GreaterThan(0);
        }
    }
}
