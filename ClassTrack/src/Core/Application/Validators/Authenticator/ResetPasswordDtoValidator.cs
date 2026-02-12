using ClassTrack.Application.DTOs;
using ClassTrack.Application.DTOs.Token;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class ResetPasswordDtoValidator:AbstractValidator<ResetPasswordDTO>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(q => q.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(200);
        }
    }
}
