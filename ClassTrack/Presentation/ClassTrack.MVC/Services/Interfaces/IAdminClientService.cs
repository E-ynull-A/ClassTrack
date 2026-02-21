using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IAdminClientService
    {
        Task<AdminDashboardVM> GetDasboardAsync();
        Task<GetUserPagedItemVM> GetUserAllAsync(int page);
        Task<ServiceResult> BanUserAsync(PostBanUserVM postBan);
        
    }
}
