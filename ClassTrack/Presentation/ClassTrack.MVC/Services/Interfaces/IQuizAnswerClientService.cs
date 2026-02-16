using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IQuizAnswerClientService
    {
        Task<ICollection<GetQuizAnswerItemVM>?> GetAllAsync(long classRoomId, long quizId, long studentId);
        Task<ServiceResult> TakeAnExamAsync(long classRoomId, PostQuizAnswerVM studentAnswer);
        Task<GetQuizAnswerVM?> GetByIdAsync(long classRoomId, long id);
        Task<ServiceResult> EvaulateAsync(PutQuizAnswerVM answerVM, long id, long classRoomId);
    }
}
