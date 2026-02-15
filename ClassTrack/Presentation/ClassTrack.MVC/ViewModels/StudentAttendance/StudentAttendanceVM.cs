namespace ClassTrack.MVC.ViewModels
{
    public record StudentAttendanceVM(

        IList<GetSimpleStudentItemVM> StudentVMs,
        IList<PostStudentAttendanceVM> AttendanceVMs = null);
    
   
}
