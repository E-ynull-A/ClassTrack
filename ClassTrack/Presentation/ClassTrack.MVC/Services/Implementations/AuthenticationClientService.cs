

using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ClassTrack.MVC.Services.Implementations
{
    public class AuthenticationClientService : IAuthenticationClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ICookieClientService _cookieService;
        private readonly IHttpContextAccessor _httpContext;

        public AuthenticationClientService(IHttpClientFactory clientFactory,
                                            ICookieClientService cookieService,
                                            IHttpContextAccessor httpContext)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
            _cookieService = cookieService;
            _httpContext = httpContext;
        }

        public async Task<ServiceResult> LoginAsync(LoginVM loginVM)
        {
           
            if (_cookieService.HasCookie("AccessToken"))
                return new ServiceResult(true);

            if (loginVM.UsernameOrEmail.Length > 256 ||
                    loginVM.UsernameOrEmail.Length < 4)
            {
                 return new ServiceResult(false,string.Empty, "The Email length is Invalid");
            }

            if (loginVM.Password.Length > 200 ||
                loginVM.Password.Length < 8)
            {
                return new ServiceResult(false,string.Empty,"The Password Lengt is Invalid");
            }

            var message = await _httpClient.PostAsJsonAsync("Accounts/Login", loginVM);

            if (!message.IsSuccessStatusCode)
            {
                var result = await message.Content.ReadFromJsonAsync<ErrorResponseVM>();

                return new ServiceResult(false,string.Empty,result.Message);
            }

            var tokens = await message.Content.ReadFromJsonAsync<ResponseTokenVM>();

            _cookieService.SetTokenCookie("AccessToken",tokens.AccessToken.AccessToken,15);
            _cookieService.SetTokenCookie("RefreshToken", tokens.RefreshToken.RefreshToken, 60 * 24 * 7); 
            
            return new ServiceResult(true);
        }
        public async Task<ServiceResult> RegisterAsync(RegisterVM registerVM)
        {

            if (registerVM.Name.Length < 3 || registerVM.Name.Length > 60)
            {
                return new ServiceResult(false, nameof(RegisterVM.Name), "The length of name is Invalid");
            }

            if (registerVM.Name.Any(n => !char.IsLetterOrDigit(n)))
            {
                return new ServiceResult(false, nameof(RegisterVM.Name), "The Name is Invalid");
            }


            if (registerVM.Surname.Length < 3 || registerVM.Surname.Length > 60)
            {
                return new ServiceResult(false, nameof(RegisterVM.Name), "The length of Surname is Invalid");
            }

            if (registerVM.Surname.Any(n => !char.IsLetterOrDigit(n)))
            {
                return new ServiceResult(false, nameof(RegisterVM.Surname), "The Surname is Invalid");
            }

            if (registerVM.Email.Length > 256 || 
                   registerVM.Email.Length < 4)
            {
                return new ServiceResult(false, string.Empty, "The Email length is Invalid");
            }
            if (registerVM.UserName.Length > 256 ||
                 registerVM.UserName.Length < 4)
            {
                return new ServiceResult(false, string.Empty, "The Username length is Invalid");
            }
            if (registerVM.Password.Length > 200 ||
                        registerVM.Password.Length < 8)
            {
                return new ServiceResult(false, string.Empty, "The Password Length is Invalid");
            }

            HttpResponseMessage message = await _httpClient
                                .PostAsJsonAsync("Accounts/Register", registerVM);         

            if (!message.IsSuccessStatusCode)
            {
                var result = await message.Content.ReadFromJsonAsync<ErrorResponseVM>();
                return new ServiceResult(false, string.Empty, result.Message);
            }

            return new ServiceResult(true);
        }
        public async Task LogoutAsync()
        {
            await _httpClient.DeleteAsync("Accounts/Logout");
            _cookieService.RemoveCookie("AccessToken");
            _cookieService.RemoveCookie("RefreshToken");
        }
        public async Task ForgetPasswordAsync(GetEmailForTokenVM emailVM)
        {
           await _httpClient.PostAsJsonAsync("Tokens/Reset", emailVM);
        }
        public async Task ResetPasswordAsync(ResetPasswordVM passwordVM)
        {
           await _httpClient.PutAsJsonAsync("Accounts/Password",passwordVM);
        }
        public async Task LeaveRoomAsync(long classRoomId)
        {
            await _httpClient.PostAsJsonAsync("Tokens/Leave", new LeaveClassRoomVM(classRoomId, "eynullam69@gmail.com"));
        }
        public async Task<ServiceResult> ConfirmLeaveAsync(LeaveTokenVM tokenVM)
        {
            HttpResponseMessage message = await _httpClient.PutAsJsonAsync("Students", tokenVM);

            if (!message.IsSuccessStatusCode)
            {
                return new ServiceResult(false, string.Empty, (await message.Content.ReadFromJsonAsync<ErrorResponseVM>())?.Message);
            }
            return new ServiceResult(true);
        }
    }
}
