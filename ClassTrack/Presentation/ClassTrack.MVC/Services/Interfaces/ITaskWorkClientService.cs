using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface ITaskWorkClientService
    {
        Task<GetTaskWorkItemWithPermissionVM?> GetAllAsync(int page, int take, long classRoomId);
        Task<GetTaskWorkVM?> GetByIdAsync(long classRoomId, long id);
        Task<ServiceResult> CreateAsync(long classRoomId, PostTaskWorkVM taskWorkVM);
        Task<ServiceResult> UpdateAsync(long id, long classRoomId, PutTaskWorkVM putTaskWork);
        Task<ICollection<GetTaskWorkAttachmentVM>?> GetAllTaskAttachmentAsync(long classRoomId, long taskWorkId);
        Task StudentSubmitAsync(long classRoomId, long taskWorkId, PutStudentTaskWorkVM studentTask);
        Task<GetStudentTaskWorkVM?> GetStudentAnswerAsync(long taskWorkId, long classRoomId);
        Task EvaulateAsync(long classRoomId, long taskWorkId, PutPointInTaskWorkVM putPointVM);
    }
}
