using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface ITaskWorkClientService
    {
        Task<GetTaskWorkItemWithPermissionVM?> GetAllAsync(int page, int take, long classRoomId);
        Task<ServiceResult> CreateAsync(long classRoomId, PostTaskWorkVM taskWorkVM);
        Task<ServiceResult> UpdateAsync(long id, long classRoomId, PutTaskWorkVM putTaskWork);
    }
}
