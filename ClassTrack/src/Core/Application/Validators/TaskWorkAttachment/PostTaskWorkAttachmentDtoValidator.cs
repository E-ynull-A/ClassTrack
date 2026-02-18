using ClassTrack.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Validators
{
    public class PostTaskWorkAttachmentDtoValidator:AbstractValidator<PostTaskWorkAttachmentDTO>
    {
        public PostTaskWorkAttachmentDtoValidator()
        {
            RuleFor(twa => twa.Files)
                .Must(f => f.Select(f => f.Length).All(l => l < 5 * 1024 * 1024 && l > 0))
                .WithMessage("The File size is invalid!");

            RuleFor(twa => twa.Files)
                .Must(ct => ct.Select(ct => ct.ContentType).All(ct => 
                           ct.Equals("image/jpeg")
                        || ct.Equals("image/png")
                        || ct.Equals("application/pdf")))
                .WithMessage("Only JPG,PNG,PDF file types could Upload");


            RuleFor(twa => twa.Files)
                .Must(f => f.Count < 15)
                .WithMessage("The Count of Files is too high!"); 
        }
    }
}
