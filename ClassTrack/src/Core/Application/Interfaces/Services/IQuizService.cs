using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface IQuizService
    {
        Task<ICollection<GetQuizItemDTO>> GetAllAsync(long classRoomId,int page, int take, params string[] includes);
        Task<GetQuizDTO> GetByIdDetailAsync(long id);
        Task<GetQuizItemDTO> GetByIdAsync(long id);

        Task CreateQuizAsync(PostQuizDTO postQuiz);
        Task UpdateQuizAsync(long id, PutQuizDTO putQuiz);
        Task DeleteQuizAsync(long id);
      
    }
}
