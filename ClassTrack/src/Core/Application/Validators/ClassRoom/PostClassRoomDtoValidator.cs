using ClassTrack.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class PostClassRoomDtoValidator:AbstractValidator<PostClassRoomDTO>
    {
        public PostClassRoomDtoValidator()
        {
            RuleFor(cr => cr.Name)
                .NotEmpty()
                .MaximumLength(150);
        }
    }
}
