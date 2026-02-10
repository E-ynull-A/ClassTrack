using ClassTrack.Application.DTOs;







namespace ClassTrack.Application.Interfaces.Services
{
    public interface ITeacherService
    {
        Task<ICollection<GetTeacherItemDTO>> GetAllAsync(long classRoomId, int page, int take);
    }
}
