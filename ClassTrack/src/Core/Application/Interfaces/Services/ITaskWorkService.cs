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
        ICollection<GetTaskWorkItemDTO> GetAllAsync(int page, int take);
        Task<ICollection<GetTaskWorkItemDTO>> GetAllByClassRoomId(int page, int take, int classRoomId);
        Task<GetTaskWorkDTO> GetById(int id);
        Task CreateTaskWork(PostTaskWorkDTO postTask);
        Task UpdateTaskWork(long id, PutTaskWorkDTO putTask);
        Task DeleteTaskWork(long id);
    }
}
