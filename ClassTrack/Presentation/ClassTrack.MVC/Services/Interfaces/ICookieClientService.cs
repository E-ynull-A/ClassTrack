namespace ClassTrack.MVC.Services.Interfaces
{
    public interface ICookieClientService
    {
        void SetTokenCookie(string key, string value, int expiration);
        void RemoveCookie(string key);
        bool HasCookie(string key);
        string GetCookieData(string key);
    }
}
