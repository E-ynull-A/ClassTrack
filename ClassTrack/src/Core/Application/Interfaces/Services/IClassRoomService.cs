using ClassTrack.Application.DTOs;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface IClassRoomService
    {
        Task<ICollection<GetClassRoomItemDTO>> GetAllAsync(int page, int take);
        Task<GetClassRoomDTO> GetByIdAsync(long id);
        Task CreateClassRoomAsync(PostClassRoomDTO postClass);
        Task UpdateClassRoomAsync(long id, PutClassRoomDTO putClass);
        Task DeleteClassRoomAsync(long id);
        Task<bool> IsOwnAsync(long classRoomId);
    }
}
