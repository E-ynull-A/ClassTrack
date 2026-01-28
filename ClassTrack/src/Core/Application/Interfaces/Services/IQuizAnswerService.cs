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
    }
}
