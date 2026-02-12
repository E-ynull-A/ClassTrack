using ClassTrack.Application.DTOs.Token;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class ResetTokenDtoValidator : AbstractValidator<ResetTokenDTO>
    {
        public ResetTokenDtoValidator()
        {
            RuleFor(q => q.Email)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(256)
                .Matches(@"^\w+([-+.']\\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
    }
}
