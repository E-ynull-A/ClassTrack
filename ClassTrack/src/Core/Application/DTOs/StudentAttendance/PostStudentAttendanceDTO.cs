using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record PostStudentAttendanceDTO(

        long ClassRoomId,
        DateTime LessonDate,
        long StudentId,
        int Attendance);
  
}
