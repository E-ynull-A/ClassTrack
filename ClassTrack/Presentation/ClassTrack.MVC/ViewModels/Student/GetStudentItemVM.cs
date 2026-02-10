

namespace ClassTrack.MVC.ViewModels
{
    public record GetStudentItemVM(
        
        long Id,
        string Name,
        string Surname,
        DateTime JoinedAt);
   
}
