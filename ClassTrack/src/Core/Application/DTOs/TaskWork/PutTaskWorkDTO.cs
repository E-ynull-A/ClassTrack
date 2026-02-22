using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record PutTaskWorkDTO(

        string Title,
        string MainPart,
        DateTime EndDate,
        DateTime StartDate,
        PostTaskWorkAttachmentDTO? AttachmentDTO,
        ICollection<long>? RemovedFileIds);
    
}
