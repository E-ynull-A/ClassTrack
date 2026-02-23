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
        Task<GetTaskWorkItemPagedDTO> GetAllAsync(int page, int take);
        Task<GetTaskWorkItemPagedDTO> GetAllByClassRoomIdAsync(int page, int take, long classRoomId);
        Task<GetStudentTaskWorkDTO> GetStudentTaskWorkAsync(long taskWorkId, long studentId);
        Task<ICollection<GetTaskWorkAttachmentDTO>> GetAttachmentAsync(long taskWorkId);
        Task<GetTaskWorkDTO> GetByIdAsync(long id);
        Task CreateTaskWorkAsync(PostTaskWorkDTO postTask);
        Task UpdateTaskWorkAsync(long id, long classRoomId, PutTaskWorkDTO putTask);
        Task DeleteTaskWorkAsync(long id);
        Task StudentSubmitAsync(PutStudentTaskWorkDTO studentSubmit, long taskWorkId);
        Task EvaulateTaskAsync(PutPointInTaskWorkDTO putPoint, long taskWorkId, long studentId);
        Task SoftQuizDeleteAsync(long id);
    }
}
