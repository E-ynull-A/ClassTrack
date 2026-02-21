using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record PostTaskWorkAttachmentDTO(ICollection<IFormFile> Files)
    {
        public PostTaskWorkAttachmentDTO():this(new Collection<IFormFile>())
        {
            
        }
    };
   
}
