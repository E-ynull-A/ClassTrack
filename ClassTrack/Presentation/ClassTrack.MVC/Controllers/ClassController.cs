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
        private readonly IAuthenticationClientService _clientService;

        public ClassController(IClassRoomService roomService,
                                IAuthenticationClientService clientService)
        {
            _roomService = roomService;
            _clientService = clientService;
        }


        public async Task<IActionResult> Logout()
        {
           await _clientService.LogoutAsync();

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

       
        [HttpPost]
        public async Task<IActionResult> DeleteClassRoom(long id)
        {
            await _roomService.DeleteClassRoomAsync(id);
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
