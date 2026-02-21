using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminClientService _adminClient;

        public AdminController(IAdminClientService adminClient)
        {
            _adminClient = adminClient;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _adminClient.GetDasboardAsync());
        }

        public async Task<IActionResult> Users(int page = 1)
        {
            if (page < 1)
                return BadRequest();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = 15;

            return View(new UsersVM(await _adminClient.GetUserAllAsync(page)));
        }

        [HttpPost]
        public async Task<IActionResult> BanUser(UsersVM usersVM)
        {
            if (!ModelState.IsValid)
            {
               return RedirectToAction("Users");
            }

            ServiceResult result = await _adminClient.BanUserAsync(usersVM.PostBan);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);               
            }

            return RedirectToAction("Users");
        }

    }
}
