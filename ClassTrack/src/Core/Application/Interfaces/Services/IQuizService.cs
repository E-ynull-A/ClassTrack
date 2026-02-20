using ClassTrack.Application.DTOs;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface IQuizService
    {
        Task<GetQuizItemPagedDTO> GetAllAsync(long classRoomId, int page, int take);
        Task<GetQuizDTO> GetByIdDetailAsync(long id);
        Task<GetQuizItemDTO> GetByIdAsync(long id);

        Task CreateQuizAsync(PostQuizDTO postQuiz);
        Task UpdateQuizAsync(long id, PutQuizDTO putQuiz);
        Task DeleteQuizAsync(long id);
      
    }
}
