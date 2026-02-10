using ClassTrack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface IStudentService
    {
        Task JoinClassAsync(JoinClassRoomDTO classRoomDTO);
        Task CalculateAvgPoint(long studentId, long classRoomId, decimal point);
        Task<ICollection<GetStudentItemDTO>> GetAllAsync(long classRoomId, int page, int take);
        Task KickAsync(long studentId, long classRoomId);
        Task PromoteAsync(long studentId, long classRoomId);
    }
}
