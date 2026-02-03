

namespace ClassTrack.MVC.ViewModels
{
    public record GetClassRoomItemVM(
        
        long Id,
        string Name,
        decimal AvgPoint,
        bool IsTeacher);
    
}
