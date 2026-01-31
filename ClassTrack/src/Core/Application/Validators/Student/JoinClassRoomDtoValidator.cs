using ClassTrack.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class JoinClassRoomDtoValidator:AbstractValidator<JoinClassRoomDTO>
    {
        public JoinClassRoomDtoValidator()
        {
            RuleFor(jc => jc.ClassKey)
                .NotEmpty()
                .Length(8)
                .Matches(@"^[a-zA-Z0-9]*$");
        }
    }
}
