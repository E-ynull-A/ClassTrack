using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IQuizClientService
    {
        Task<GetQuizItemWithPermissionVM> GetAllAsync(long classRoomId);
        Task<GetQuizItemVM> GetByIdAsync(long id, long classRoomId);
        Task<GetQuizVM?> GetQuizForStudentAsync(long classRoomId, long quizId);
        Task<ServiceResult> CreateQuizAsync(PostQuizVM quizVM);
        Task<ServiceResult> UpdateQuizAsync(PutQuizVM quizVM, long id);
    }
}
