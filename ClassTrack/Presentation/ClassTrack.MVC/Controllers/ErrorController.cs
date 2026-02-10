using ClassTrack.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IAuthenticationClientService _authenticationClient;

        public ErrorController(IAuthenticationClientService authenticationClient)
        {
            _authenticationClient = authenticationClient;
        }
        public async Task<IActionResult> Index(string messageError)
        {
            if (messageError.Contains("Unauthorized"))
            {
                await _authenticationClient.LogoutAsync();
                return RedirectToAction("Login", "Home");
            }
               
            return View(model: messageError);
        }
    }
}
