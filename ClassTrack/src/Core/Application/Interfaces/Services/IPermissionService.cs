using ClassTrack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface IPermissionService
    {
        Task<IsTeacherDTO> IsTeacherAsync(long classRoomId);
        Task<bool> IsAdminAsync(string emailOrUserName);
    }
}
