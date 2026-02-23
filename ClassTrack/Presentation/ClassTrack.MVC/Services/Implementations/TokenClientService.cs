using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Services.Implementations
{
    public class TokenClientService:ITokenClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ICookieClientService _cookieService;

        public TokenClientService(IHttpClientFactory clientFactory,
                                   ICookieClientService cookieService)
        {
            _httpClient = clientFactory.CreateClient("RefreshTokenClient");
            _cookieService = cookieService;
        }
        public async Task<ResponseTokenVM> GetTokensAsync(string rToken)
        {
           ResponseTokenVM? tokenVM = await _httpClient.GetFromJsonAsync<ResponseTokenVM>($"Tokens/Refresh/{rToken}");

            if (tokenVM is null)
                throw new Exception("The Tokens isn't Created!");

            _cookieService.SetTokenCookie("AccessToken", tokenVM.AccessToken.AccessToken,7);
            _cookieService.SetTokenCookie("RefreshToken", tokenVM.RefreshToken.RefreshToken, 7);


            return tokenVM;
        }

 
    }
}
