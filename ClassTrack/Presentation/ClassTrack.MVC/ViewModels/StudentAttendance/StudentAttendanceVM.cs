namespace ClassTrack.MVC.ViewModels
{
    public record StudentAttendanceVM(

        IList<GetStudentItemVM> StudentVMs,
        IList<PostStudentAttendanceVM> AttendanceVMs = null);
    
   
}
