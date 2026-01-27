using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetTaskWorkItemDTO(
        long Id,

        string Title,
        DateTime EndDate,
        DateTime StartDate,
        string MainPart);
    
}
