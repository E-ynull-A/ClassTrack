

namespace ClassTrack.MVC.ViewModels
{
    public record GetQuizItemVM(
        
       long Id,
       string Name,
       DateTime StartTime,
       TimeSpan Duration);
    
}
