using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IClassRoomClientService
    {
        Task<IEnumerable<GetClassRoomItemVM>> GetAllAsync();
        Task<GetClassRoomWithPermissionVM> GetByIdAsync(long id);
        Task CreateClassRoomAsync(PostClassRoomVM classRoomVM);
        Task DeleteClassRoomAsync(long id);
    }
}
