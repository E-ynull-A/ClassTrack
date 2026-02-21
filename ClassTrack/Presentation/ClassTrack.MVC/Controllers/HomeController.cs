using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthenticationClientService _clientService;

        public HomeController(IAuthenticationClientService clientService)
        {
            _clientService = clientService;
        }
        public IActionResult Index()
        {                
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]              
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if(!ModelState.IsValid)
                return View(loginVM);

            ServiceResult serviceResult = await _clientService.LoginAsync(loginVM);

            if (!serviceResult.Ok)
            {
                ModelState.AddModelError(serviceResult.ErrorKey,
                                         serviceResult.ErrorMessage);
                return View(loginVM);
            }

            return RedirectToAction("Dashboard", "Class"); 
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            ServiceResult result = await _clientService.RegisterAsync(registerVM);

            if(!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);
                return View(registerVM);
            }

            return RedirectToAction("Login");
        }

        public IActionResult Forget()
        {
            return View("Forgot_Password");
        }

        [HttpPost]
        public async Task<IActionResult> Forget(GetEmailForTokenVM tokenVM)
        {
           await _clientService.ForgetPasswordAsync(tokenVM);
           return RedirectToAction("Login");
        }

        public IActionResult Reset(string email)
        {
            return View("Reset_Password");
        }

        [HttpPost]
        public async Task<IActionResult> Reset(ResetPasswordVM passwordVM)
        {
           await _clientService.ResetPasswordAsync(passwordVM);
           return RedirectToAction("Login");
        }

    }
}
