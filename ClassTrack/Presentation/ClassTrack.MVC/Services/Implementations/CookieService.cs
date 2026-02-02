using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Implementations
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _accessor;

        public CookieService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public void SetTokenCookie(string key,string value,int expiration)
        {

            var cookieOpt = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(expiration)
            };

            _accessor.HttpContext.Response.Cookies.Append(key, value, cookieOpt);         
        }
    }
}
