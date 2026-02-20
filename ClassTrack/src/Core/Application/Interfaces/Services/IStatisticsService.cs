using ClassTrack.Application.DTOs;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface IStatisticsService
    {
        Task<GetStatisticsDTO> GetCountAsync();
        Task<GetUserPagedItemDTO> GetAllUserAsync(int page, int take);
    }
}
