using ClassTrack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface IQuizService
    {
        Task<ICollection<GetQuizItemDTO>> GetAllAsync(int page, int take, params string[] includes);
        Task<GetQuizDTO> GetByIdAsync(long id);
    }
}
