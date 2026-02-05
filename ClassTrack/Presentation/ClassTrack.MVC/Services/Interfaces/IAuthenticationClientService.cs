using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IAuthenticationClientService
    {
        Task Register(RegisterVM registerVM);
        Task<bool> LoginAsync(LoginVM loginVM);
        Task LogoutAsync();
    }
}
