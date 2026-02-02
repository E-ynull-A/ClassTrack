namespace ClassTrack.MVC.Services.Interfaces
{
    public interface ICookieService
    {
        void SetTokenCookie(string key, string value, int expiration);
    }
}
