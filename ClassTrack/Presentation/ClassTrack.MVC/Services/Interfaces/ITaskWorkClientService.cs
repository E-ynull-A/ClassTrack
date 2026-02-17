using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface ITaskWorkClientService
    {
        Task<ServiceResult> CreateAsync(long classRoomId, PostTaskWorkVM taskWorkVM);
    }
}
