using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Implementations
{
    public class CookieClientService : ICookieClientService
    {
        private readonly IHttpContextAccessor _accessor;

        public CookieClientService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public void SetTokenCookie(string key,string value,int expiration)
        {

            var cookieOpt = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true, 
                Expires = DateTime.UtcNow.AddDays(expiration),
                Path = "/"
            };

            _accessor.HttpContext.Response.Cookies.Append(key, value, cookieOpt);         
        }

        public void RemoveCookie(string key)
        {
            _accessor.HttpContext.Response.Cookies.Delete(key);
        }

        public bool HasCookie(string key)
        {
            return _accessor.HttpContext.Request.Cookies.ContainsKey(key);
        }

        public string GetCookieData(string key)
        {
           return _accessor.HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == key).Value;
        }
    }
}
