using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IQuizAnswerClientService
    {
        Task<ServiceResult> TakeAnExamAsync(long classRoomId, PostQuizAnswerVM studentAnswer);
    }
}
