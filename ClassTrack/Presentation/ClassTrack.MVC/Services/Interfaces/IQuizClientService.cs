using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IQuizClientService
    {
        public Task<IEnumerable<GetQuizItemVM>> GetAllAsync(long id);
        Task<ServiceResult> CreateQuizAsync(PostQuizVM quizVM);
        Task<ServiceResult> PostQuizAsync(PutQuizVM quizVM, long id);
    }
}
