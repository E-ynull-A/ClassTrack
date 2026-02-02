

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
            _httpClient = clientFactory.CreateClient("ClassTrack");
            _cookieService = cookieService;
        }

        public async Task LoginAsync(LoginVM loginVM)
        {
            var message = await _httpClient.PostAsJsonAsync("Accounts/Login", loginVM);

            var tokens = await message.Content.ReadFromJsonAsync<ResponseTokenVM>();

            _cookieService.SetTokenCookie("AccessToken",tokens.AccessToken.AccessToken,15);
            _cookieService.SetTokenCookie("RefreshToken", tokens.RefreshToken.RefreshToken, 60 * 24 * 7);

            
        }

        public Task Register(RegisterVM registerVM)
        {
            throw new NotImplementedException();
        }
    }
}
