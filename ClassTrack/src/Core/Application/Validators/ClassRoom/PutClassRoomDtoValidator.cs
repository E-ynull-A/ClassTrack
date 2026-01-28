using ClassTrack.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class PutClassRoomDtoValidator:AbstractValidator<PutClassRoomDTO>
    {
        public PutClassRoomDtoValidator()
        {
            RuleFor(cr=>cr.Name)
                .NotEmpty()
                .MaximumLength(150);
        }
    }
}
