namespace ClassTrack.MVC.ViewModels
{
    public record GetQuizItemWithPermissionVM(
        
        IEnumerable<GetQuizItemVM> ItemVM,
        IsTeacherVM IsTeacherVM);
    
}
