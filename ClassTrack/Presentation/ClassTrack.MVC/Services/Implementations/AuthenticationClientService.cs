

using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Threading.Tasks;


namespace ClassTrack.MVC.Services.Implementations
{
    public class AuthenticationClientService : IAuthenticationClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ICookieClientService _cookieService;

        public AuthenticationClientService(IHttpClientFactory clientFactory,
                                            ICookieClientService cookieService)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
            _cookieService = cookieService;
        }

        public async Task<ServiceResult> LoginAsync(LoginVM loginVM)
        {
           
            if (_cookieService.HasCookie("AccessToken"))
                return new ServiceResult(true);

            if (loginVM.UsernameOrEmail.Length > 256 ||
                    loginVM.UsernameOrEmail.Length < 4)
            {
                 return new ServiceResult(false,nameof(LoginVM.UsernameOrEmail),
                                       "The Username Or Email Length is Wrong!");
            }

            if (loginVM.Password.Length > 200 ||
                loginVM.Password.Length < 8)
            {
                return new ServiceResult(false,nameof(LoginVM.Password),
                              "The Password Length is Wrong!");
            }

            var message = await _httpClient.PostAsJsonAsync("Accounts/Login", loginVM);

            if (!message.IsSuccessStatusCode)
            {
                return new ServiceResult(false,string.Empty,"Username, Email or Password is Incorrect");
            }

            var tokens = await message.Content.ReadFromJsonAsync<ResponseTokenVM>();

            _cookieService.SetTokenCookie("AccessToken",tokens.AccessToken.AccessToken,15);
            _cookieService.SetTokenCookie("RefreshToken", tokens.RefreshToken.RefreshToken, 60 * 24 * 7); 
            
            return new ServiceResult(true);
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
        public async Task ForgetPasswordAsync(ResetTokenVM resetToken)
        {
           await _httpClient.PostAsJsonAsync("Tokens/Reset",resetToken);
        }
        public async Task ResetPasswordAsync(ResetPasswordVM passwordVM)
        {
           await _httpClient.PutAsJsonAsync("Accounts/Password",passwordVM);
        }


    }
}
