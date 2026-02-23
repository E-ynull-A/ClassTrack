using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IAdminClientService
    {
        Task<AdminDashboardVM> GetDasboardAsync();
        Task<GetUserPagedItemVM> GetUserAllAsync(int page);
        Task<ServiceResult> BanUserAsync(PostBanUserVM postBan);
        Task<GetQuizItemPagedVM> GetAllAsync(int page, int take);
        Task<GetTaskWorkItemPagedVM?> GetTaskWorkAllAsync(int page = 1, int take = 5);
        Task DeleteQuizAsync(long id);
        Task DeleteTaskAsync(long id);


    }
}
