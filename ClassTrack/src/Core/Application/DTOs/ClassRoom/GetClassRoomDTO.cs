using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetClassRoomDTO(

        long Id,
        string Name,
        string PrivateKey,
        decimal AvgPoint,
        int MemberCount);
    
}
