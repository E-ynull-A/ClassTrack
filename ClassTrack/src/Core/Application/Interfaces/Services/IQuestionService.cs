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


        Task CreateChoiceQuestionAsync(PostChoiceQuestionDTO postChoice);
        Task CreateOpenQuestionAsync(PostOpenQuestionDTO postOpen);

        Task UpdateChoiceQuestionAsync(long id, PutChoiceQuestionDTO putChoice);


    }
}
