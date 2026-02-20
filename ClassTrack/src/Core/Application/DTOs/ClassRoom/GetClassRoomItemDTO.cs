using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetClassRoomItemDTO(

        long Id,
        string Name,
        decimal AvgPoint,
        int StudentCount,
        ICollection<string> TeacherFullNames,
        DateTime CreatedAt);

}
