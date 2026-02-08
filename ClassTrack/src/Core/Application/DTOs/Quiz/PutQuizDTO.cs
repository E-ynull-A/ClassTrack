using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record PutQuizDTO(

       string Name,
       DateTime StartTime,
       double Duration,
       long ClassRoomId);
    
}
