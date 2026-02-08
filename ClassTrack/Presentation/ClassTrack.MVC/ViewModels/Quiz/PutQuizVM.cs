

namespace ClassTrack.MVC.ViewModels
{
    public record PutQuizVM(

       string Name,
       DateTime StartTime,
       double Duration,
       long ClassRoomId);
    
}
