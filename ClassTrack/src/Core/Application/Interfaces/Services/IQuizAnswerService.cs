using ClassTrack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface IQuizAnswerService
    {
        Task<ICollection<GetQuizAnswerItemDTO>> GetAllByStudentIdAsync(long studentId, int page, int take);
        Task<GetQuizAnswerDTO> GetByIdAsync(long id);
        Task TakeAnExamAsync(PostQuizAnswerDTO answerDTO);
        Task EvaluateAnswerAsync(long id, PutQuizAnswerDTO answerDTO);
    }
}
