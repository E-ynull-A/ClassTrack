using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IQuizClientService
    {
        Task<GetQuizItemWithPermissionVM> GetAllAsync(long classRoomId,int page,int take);
        Task<GetQuizItemVM> GetByIdAsync(long id, long classRoomId);
        Task<GetQuizVM?> GetQuizInDetailAsync(long quizId, long classRoomId = 0);
        Task<ServiceResult> CreateQuizAsync(PostQuizVM quizVM);
        Task<ServiceResult> UpdateQuizAsync(PutQuizVM quizVM, long id);
        Task SoftDeleteQuizAsync(long classRoomId, long id);
        Task<ICollection<GetStudentQuizResultVM>> GetResultAsync(long classRoomId);
    }
}
