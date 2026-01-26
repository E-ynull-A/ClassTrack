using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetStudentAttendanceItemDTO(

        long ClassRoomId,
        DateOnly LessonDate,
        long StudentId,
        string StudentName,
        string StudentSurname,
        int Attendance);
    
}
