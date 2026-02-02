using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IAuthenticationClientService
    {
        Task Register(RegisterVM registerVM);
        Task LoginAsync(LoginVM loginVM);
    }
}
