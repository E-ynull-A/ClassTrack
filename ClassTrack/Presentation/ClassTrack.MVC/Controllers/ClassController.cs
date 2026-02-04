using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassRoomService _roomService;

        public ClassController(IClassRoomService roomService)
        {
            _roomService = roomService;
        }


        public async Task<IActionResult> Logout()
        {
           await _roomService.LogoutAsync();

           return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> ClassRoom(long id)
        {          
            return View(await _roomService.GetByIdAsync(id));
        }

        public async Task<IActionResult> Dashboard()
        {            
            return View(new DashboardVM(await _roomService.GetAllAsync()));
        }

        [HttpPost]
        public async Task<IActionResult> Dashboard(DashboardVM dashboardVM)
        {
            if (!ModelState.IsValid)
            {
                return View(new DashboardVM(await _roomService.GetAllAsync(),
                                                    dashboardVM.PostClass));
            }

            await _roomService.CreateClassRoomAsync(dashboardVM.PostClass);

            return RedirectToAction("Dashboard");
        }

        

        public IActionResult QuizEditor()
        {
            return View();
        }

        public IActionResult Attendance()
        {
            return View();
        }

        public IActionResult Members()
        {
            return View();
        }
    }
}
