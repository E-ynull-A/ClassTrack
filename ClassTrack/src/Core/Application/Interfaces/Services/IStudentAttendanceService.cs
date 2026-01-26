using ClassTrack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface IStudentAttendanceService
    {
        Task CreateAttendanceAsync(ICollection<PostStudentAttendanceDTO> attendanceDTOs);
        Task<ICollection<GetStudentAttendanceItemDTO>> GetAllAsync(int page, int take);
        Task UpdateAttendanceAsync(ICollection<PutStudentAttendanceDTO> attendanceDTOs);
        Task DeleteAttendanceAsync(long id);
    }
}
