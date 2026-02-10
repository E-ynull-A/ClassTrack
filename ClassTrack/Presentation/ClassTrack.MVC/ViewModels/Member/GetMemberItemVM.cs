namespace ClassTrack.MVC.ViewModels
{
    public record GetMemberItemVM(
        ICollection<GetStudentItemVM> Students,
        ICollection<GetTeacherItemVM> Teachers);
 
}
