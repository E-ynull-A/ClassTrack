using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetTaskWorkAttachmentDTO(
        long Id,
        string FileName,
        string FileUrl,
        string FileType);
   
}
