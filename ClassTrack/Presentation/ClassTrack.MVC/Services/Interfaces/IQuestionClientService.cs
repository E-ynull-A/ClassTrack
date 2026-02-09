using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IQuestionClientService
    {
        Task<ICollection<GetQuestionItemVM>> GetAllAsync(long classRoomId, long quizId);
        Task<GetChoiceQuestionForUpdateVM?> GetChoiceForUpdateAsync(long classRoomId, long quizId, long id);
        Task<GetOpenQuestionForUpdateVM?> GetOpenForUpdateAsync(long classRoomId, long quizId, long id);
        Task<UpdateQuestionVM> GetUpdateAsync(long id, long quizId, long classRoomId, string questionType);
        Task<ServiceResult> UpdateAsync(long id, UpdateQuestionVM questionVM, long quizId, long classRoomId, string questionType);
    }
}
