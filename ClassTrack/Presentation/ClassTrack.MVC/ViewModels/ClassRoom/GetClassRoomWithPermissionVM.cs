namespace ClassTrack.MVC.ViewModels
{
    public record GetClassRoomWithPermissionVM(
        
        IsTeacherVM IsTeacher,
        GetClassRoomVM? ClassRoom);
   
}
