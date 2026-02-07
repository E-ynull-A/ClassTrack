

namespace ClassTrack.MVC.ViewModels
{
    public record PutQuizVM(

       string Name,
       DateTime StartTime,
       int Duration,
       long ClassRoomId);
    
}
