using ClassTrack.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class RegisterDtoValidator:AbstractValidator<RegisterDTO>
    {
        public RegisterDtoValidator()
        {
            RuleFor(q => q.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(60)
                .Matches(@"^[A-Za-z]*$");

            RuleFor(q => q.Surname)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(60)
                .Matches(@"^[A-Za-z]*$");

            RuleFor(q => q.Email)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(256)
                .Matches(@"^\w+([-+.']\\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            RuleFor(q => q.UserName)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(256)
                .Matches(@"^[A-Za-z0-9-._+]*$");

            RuleFor(q => q.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(200);

            RuleFor(q => q)
                .Must(q => q.Password == q.ConfirmPassword)
                .Must(q=>DateOnly.FromDateTime(DateTime.UtcNow) >= q.BirthDate.AddYears(q.Age) &&
                        DateOnly.FromDateTime(DateTime.UtcNow) < q.BirthDate.AddYears(q.Age + 1))
                .WithMessage("Enter correctly your Birthday or Age");
        }

    }
}
