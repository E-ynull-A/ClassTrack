using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record PutStudentAttendanceDTO(

        long Id,
        long ClassRoomId,
        DateTime LessonDate,
        long StudentId,
        int Attendance);
  
}
