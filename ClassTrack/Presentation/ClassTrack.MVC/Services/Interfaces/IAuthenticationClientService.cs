using ClassTrack.MVC.ViewModels;


namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IAuthenticationClientService
    {
        Task<ServiceResult> RegisterAsync(RegisterVM registerVM);
        Task<ServiceResult> LoginAsync(LoginVM loginVM);
        Task LogoutAsync();
        Task ForgetPasswordAsync(ResetTokenVM resetToken);
        Task ResetPasswordAsync(ResetPasswordVM passwordVM);
    }
}
