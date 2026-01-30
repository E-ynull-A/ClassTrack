using ClassTrack.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class PostQuizAnswerDtoValidator : AbstractValidator<PostQuizAnswerDTO>
    {
        public PostQuizAnswerDtoValidator()
        {
            RuleFor(qa => qa.StudentQuizId)
                .NotEmpty()
                .GreaterThan(0);



            RuleFor(qa => qa.Answers)
                .NotEmpty()
                .Must(a=>a.All(a=>a.QuestionId > 0))
                .Must(a => a.Select(a => a.QuestionId).Count() > 0)
                .Must(a => a.Select(a => a.QuestionId).Distinct().Count()
                                  == a.Select(a => a.QuestionId).Count()
                                  && a.Select(a => a.AnswerId).Distinct().Count()
                                  == a.Select(a => a.AnswerId).Count())
                .WithMessage("No Dublicate Ids!");

        }
    }
}
