using ClassTrack.MVC.ViewModels;


namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IAuthenticationClientService
    {
        Task Register(RegisterVM registerVM);
        Task<ServiceResult> LoginAsync(LoginVM loginVM);
        Task LogoutAsync();
        Task ForgetPasswordAsync(ResetTokenVM resetToken);
        Task ResetPasswordAsync(ResetPasswordVM passwordVM);
    }
}
