

using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;


namespace ClassTrack.MVC.Services.Implementations
{
    public class AuthenticationClientService : IAuthenticationClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ICookieService _cookieService;

        public AuthenticationClientService(IHttpClientFactory clientFactory,
                                            ICookieService cookieService)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
            _cookieService = cookieService;
        }

        public async Task<bool> LoginAsync(LoginVM loginVM)
        {
           
            if (_cookieService.HasCookie("AccessToken"))
                return true;


            var message = await _httpClient.PostAsJsonAsync("Accounts/Login", loginVM);

            if (!message.IsSuccessStatusCode)
            {
                return false;
            }

            var tokens = await message.Content.ReadFromJsonAsync<ResponseTokenVM>();

            _cookieService.SetTokenCookie("AccessToken",tokens.AccessToken.AccessToken,15);
            _cookieService.SetTokenCookie("RefreshToken", tokens.RefreshToken.RefreshToken, 60 * 24 * 7); 
            
            return true;
        }
        public Task Register(RegisterVM registerVM)
        {
            throw new NotImplementedException();
        }

        public async Task LogoutAsync()
        {
            await _httpClient.DeleteAsync("Accounts/Logout");
            _cookieService.RemoveCookie("AccessToken");
            _cookieService.RemoveCookie("RefreshToken");
        }


    }
}
