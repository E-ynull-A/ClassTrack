using ClassTrack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface ITaskWorkService
    {
        Task<ICollection<GetTaskWorkItemDTO>> GetAllAsync(int page, int take);
        Task<ICollection<GetTaskWorkItemDTO>> GetAllByClassRoomIdAsync(int page, int take, long classRoomId);
        Task<GetTaskWorkDTO> GetByIdAsync(long id);
        Task CreateTaskWorkAsync(PostTaskWorkDTO postTask);
        Task UpdateTaskWorkAsync(long id, PutTaskWorkDTO putTask);
        Task DeleteTaskWorkAsync(long id);
    }
}
