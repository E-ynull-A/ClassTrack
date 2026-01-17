using ClassTrack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface IQuestionService
    {
        Task<ICollection<GetQuestionItemDTO>> GetAllAsync(int page, int take, params string[] includes);

        Task<GetQuestionDTO> GetByIdAsync(long id);
    }
}
