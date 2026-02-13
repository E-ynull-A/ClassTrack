



namespace ClassTrack.MVC.ViewModels
{
    public record PostStudentAttendanceVM(

        long ClassRoomId,
        DateTime LessonDate,
        long StudentId,
        int Attendance);
  
}
