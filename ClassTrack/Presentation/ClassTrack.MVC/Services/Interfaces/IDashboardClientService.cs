using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IDashboardClientService
    {
        Task<AdminDashboardVM> GetDasboardAsync();
        Task<GetUserPagedItemVM> GetUserAllAsync(int page);
        Task<ServiceResult> BanUserAsync(PostBanUserVM postBan);
    }
}
