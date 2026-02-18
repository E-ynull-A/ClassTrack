using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IClassRoomClientService
    {
        Task<IEnumerable<GetClassRoomItemVM>> GetAllAsync();
        Task<GetClassRoomWithPermissionVM> GetByIdAsync(long id);
        Task<ServiceResult> CreateClassRoomAsync(PostClassRoomVM classRoomVM);
        Task<ServiceResult> JoinClassRoomAsync(JoinClassRoomVM joinClass);
        Task<ServiceResult> UpdateClassRoomAsync(long classRoomId, PutClassRoomVM roomVM);
        Task DeleteClassRoomAsync(long id);
    }
}
