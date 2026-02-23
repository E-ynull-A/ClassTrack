using ClassTrack.MVC.ViewModels;


namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IAuthenticationClientService
    {
        Task<ServiceResult> RegisterAsync(RegisterVM registerVM);
        Task<ServiceResult> LoginAsync(LoginVM loginVM);
        Task LogoutAsync();
        Task ForgetPasswordAsync(GetEmailForTokenVM resetToken);
        Task ResetPasswordAsync(ResetPasswordVM passwordVM);
        Task<ServiceResult> ConfirmLeaveAsync(LeaveTokenVM tokenVM);
        Task LeaveRoomAsync(long classRoomId);
        Task<bool> IsAdminAsync(string userNameOrEmail = null);
    }
}
