using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IClassRoomService
    {
        Task<IEnumerable<GetClassRoomItemVM>> GetAllAsync(int page = 1, int take = 5);

        Task<GetClassRoomWithPermissionVM> GetByIdAsync(long id);
    }
}
