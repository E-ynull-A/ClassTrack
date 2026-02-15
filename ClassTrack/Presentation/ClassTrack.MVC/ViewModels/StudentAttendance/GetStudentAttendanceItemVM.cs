



namespace ClassTrack.MVC.ViewModels
{
    public record GetStudentAttendanceItemVM(

        long ClassRoomId,
        DateTime LessonDate,
        long StudentId,
        string StudentName,
        string StudentSurname,
        int Attendance);
    
}
