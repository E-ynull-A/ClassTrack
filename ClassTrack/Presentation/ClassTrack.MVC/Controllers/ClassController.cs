using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassRoomClientService _roomService;
        private readonly IAuthenticationClientService _clientService;

        public ClassController(IClassRoomClientService roomService,
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
            if (id < 1)
                return BadRequest();

            GetClassRoomWithPermissionVM getClassRoom = await _roomService.GetByIdAsync(id);

            if (getClassRoom.IsTeacher.IsTeacher)
                return View("TeacherClassRoom", getClassRoom);

            return View("StudentClassRoom",getClassRoom);
        }
        public async Task<IActionResult> Dashboard()
        {            
            return View(new DashboardVM(await _roomService.GetAllAsync()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateClassRoom(DashboardVM dashboardVM)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Dashboard),new DashboardVM(await _roomService.GetAllAsync(),
                                                    dashboardVM.PostClass));
            }
      
            ServiceResult result = await _roomService.CreateClassRoomAsync(dashboardVM.PostClass);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> JoinClassRoom(DashboardVM dashboardVM)
        {
            ModelState.Remove(nameof(DashboardVM.PostClass));

            if (!ModelState.IsValid)
            {
                return View(nameof(Dashboard), new DashboardVM(await _roomService.GetAllAsync(),
                                                    dashboardVM.PostClass));
            }

            ServiceResult result = await _roomService.JoinClassRoomAsync(dashboardVM.JoinClass);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);
                return View(nameof(Dashboard), new DashboardVM(await _roomService.GetAllAsync(),
                                                   dashboardVM.PostClass));
            }

            return RedirectToAction("Dashboard");
        }
        
       
        public async Task<IActionResult> DeleteClassRoom(long id)
        {
            if (id < 1)
                return BadRequest();

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
