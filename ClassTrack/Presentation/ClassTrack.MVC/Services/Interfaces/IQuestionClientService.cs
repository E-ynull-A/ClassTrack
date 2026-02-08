using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IQuestionClientService
    {
        Task<ICollection<GetQuestionItemVM>> GetAllAsync(long classRoomId, long quizId);
    }
}
