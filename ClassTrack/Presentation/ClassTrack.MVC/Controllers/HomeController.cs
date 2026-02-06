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

            if(loginVM.UsernameOrEmail.Length>256 ||
                loginVM.UsernameOrEmail.Length < 4)
            {
                ModelState.AddModelError(nameof(LoginVM.UsernameOrEmail),
                                "The Username Or Email Length is Wrong!");

                return View(loginVM);
            }

            if(loginVM.Password.Length > 200 ||
                loginVM.Password.Length < 8)
            {
                ModelState.AddModelError(nameof(LoginVM.UsernameOrEmail),
                              "The Password Length is Wrong!");

                return View(loginVM);
            }

            if(await _clientService.LoginAsync(loginVM))
            {
                return RedirectToAction("Dashboard", "Class");
            }

            return View(loginVM);
        }

    }
}
