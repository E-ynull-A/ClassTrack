using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetTaskWorkDTO(

        long Id,
        long ClassRoomId,
        string Title,
        string MainPart,
        DateTime EndDate,
        DateTime StartDate);
   
}
