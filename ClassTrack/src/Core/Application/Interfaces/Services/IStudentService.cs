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
        Task<ICollection<GetStudentQuizResultDTO>> GetStudentResultAsync(long classRoomId);
        Task JoinClassAsync(JoinClassRoomDTO classRoomDTO);
        Task CalculateAvgPoint(long studentId, long classRoomId);
        Task<ICollection<GetSimpleStudentItemDTO>> GetBriefAllAsync(long classRoomId, int page, int take);
        Task<ICollection<GetStudentItemDTO>> GetAllAsync(long classRoomId, int page, int take);
        Task KickAsync(long studentId, long classRoomId);
        Task PromoteAsync(long studentId, long classRoomId);
        Task RequestLeaveAsync(LeaveTokenDTO tokenDTO);
    }
}
