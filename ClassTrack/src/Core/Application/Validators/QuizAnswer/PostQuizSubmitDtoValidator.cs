using ClassTrack.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class PostQuizSubmitDtoValidator:AbstractValidator<PostQuizSubmitDTO>
    {
        public PostQuizSubmitDtoValidator()
        {
            RuleFor(qs => qs)
                .Must(qs => qs.QuestionId > 0 && qs.AnswerId > 0);

            RuleFor(qs => qs.QuestionId)
                .NotEmpty();

            RuleFor(qs => qs)
                .Must(qs => qs.AnswerText is not null && qs.AnswerId is null && qs.AnswerIds is null ||
                            qs.AnswerText is null && qs.AnswerId is not null && qs.AnswerIds is null ||
                            qs.AnswerText is null && qs.AnswerId is null && qs.AnswerIds is not null)
                ;

        }
    }
}
