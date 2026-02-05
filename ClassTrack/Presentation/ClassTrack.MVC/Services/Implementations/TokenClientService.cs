using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Services.Implementations
{
    public class TokenClientService:ITokenClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ICookieService _cookieService;

        public TokenClientService(IHttpClientFactory clientFactory,
                                   ICookieService cookieService)
        {
            _httpClient = clientFactory.CreateClient("RefreshTokenClient");
            _cookieService = cookieService;
        }
        public async Task<ResponseTokenVM> GetTokensAsync()
        {
           ResponseTokenVM? tokenVM = await _httpClient.GetFromJsonAsync<ResponseTokenVM>("Tokens/Refresh");

            if (tokenVM is null)
                throw new Exception("The Tokens isn't Created!");

            _cookieService.SetTokenCookie("AccessToken", tokenVM.AccessToken.AccessToken,7);
            _cookieService.SetTokenCookie("RefreshToken", tokenVM.RefreshToken.RefreshToken, 7);


            return tokenVM;
        }
 
    }
}
