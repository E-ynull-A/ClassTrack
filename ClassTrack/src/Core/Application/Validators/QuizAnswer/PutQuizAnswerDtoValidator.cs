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
            RuleFor(qa => qa.StudentQuizId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(qa => qa.Answers)
                .NotEmpty()
                .Must(a => a.Select(a => a.QuizAnswerId).Distinct().Count()
                                        ==
                         a.Select(a => a.QuizAnswerId).Count());
        }
    }
}
