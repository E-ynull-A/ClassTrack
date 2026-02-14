using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface ITokenClientService
    {
        Task<ResponseTokenVM> GetTokensAsync(string oldRefresh);
    }
}
