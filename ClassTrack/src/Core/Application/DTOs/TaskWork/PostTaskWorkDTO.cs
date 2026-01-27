using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record PostTaskWorkDTO(

        long ClassRoomId,
        string Title,
        string MainPart,
        DateTime EndDate,
        DateTime StartDate);

   
}
