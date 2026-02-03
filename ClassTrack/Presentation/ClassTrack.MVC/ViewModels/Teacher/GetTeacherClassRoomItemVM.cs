namespace ClassTrack.MVC.ViewModels
{
    public record GetTeacherClassRoomItemVM
        (
          ICollection<GetClassRoomItemVM> TeacherClasses
        );
   
}
