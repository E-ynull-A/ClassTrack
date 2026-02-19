

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
                 return new ServiceResult(false,string.Empty,string.Empty);
            }

            if (loginVM.Password.Length > 200 ||
                loginVM.Password.Length < 8)
            {
                return new ServiceResult(false,string.Empty,"");
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
            //if(registerVM.Name.Length < 3 || registerVM.Name.Length > 60)
            //{
            //    return new ServiceResult(false, nameof(RegisterVM.Name), "The length of name is Invalid");
            //}

            //RuleFor(q => q.Name)
            //    .NotEmpty()
            //    .MinimumLength(3)
            //    .MaximumLength(60)
            //    .Matches(@"^[A-Za-z]*$");

            //RuleFor(q => q.Surname)
            //    .NotEmpty()
            //    .MinimumLength(3)
            //    .MaximumLength(60)
            //    .Matches(@"^[A-Za-z]*$");

            //RuleFor(q => q.Email)
            //    .NotEmpty()
            //    .MinimumLength(4)
            //    .MaximumLength(256)
            //    .Matches(@"^\w+([-+.']\\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            //RuleFor(q => q.UserName)
            //    .NotEmpty()
            //    .MinimumLength(4)
            //    .MaximumLength(256)
            //    .Matches(@"^[A-Za-z0-9-._+]*$");

            //RuleFor(q => q.Password)
            //    .NotEmpty()
            //    .MinimumLength(8)
            //    .MaximumLength(200);

            //RuleFor(q => q)
            //    .Must(q => q.Password == q.ConfirmPassword)
            //    .Must(q => DateOnly.FromDateTime(DateTime.UtcNow) >= q.BirthDate.AddYears(q.Age) &&
            //            DateOnly.FromDateTime(DateTime.UtcNow) < q.BirthDate.AddYears(q.Age + 1))
            //    .WithMessage("Enter correctly your Birthday or Age");


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
