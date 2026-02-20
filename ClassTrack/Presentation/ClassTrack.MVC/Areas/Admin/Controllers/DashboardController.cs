using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IDashboardClientService _dashboardClient;

        public DashboardController(IDashboardClientService dashboardClient)
        {
            _dashboardClient = dashboardClient;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dashboardClient.GetDasboardAsync());
        }

        public async Task<IActionResult> Users(int page = 1)
        {
            if (page < 1)
                return BadRequest();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = 15;

            return View(await _dashboardClient.GetUserAllAsync(page));
        }

        [HttpPost]
        public async Task<IActionResult> BanUser(PostBanUserVM postBan)
        {
            if (!ModelState.IsValid)
            {
               return RedirectToAction("Users");
            }

            ServiceResult result = await _dashboardClient.BanUserAsync(postBan);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);               
            }

            return RedirectToAction("Users");
        }
    }
}
