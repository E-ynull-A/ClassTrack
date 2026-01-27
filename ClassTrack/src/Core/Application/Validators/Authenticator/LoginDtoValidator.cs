using ClassTrack.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class LoginDtoValidator:AbstractValidator<LoginDTO>
    {
        public LoginDtoValidator()
        {
            RuleFor(l => l.UsernameOrEmail)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(256)
                .Matches(@"^[A-Za-z0-9-._@+]*$");

            RuleFor(r => r.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(200);
        }
    }
}
