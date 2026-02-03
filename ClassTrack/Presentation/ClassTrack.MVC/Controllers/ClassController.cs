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
        public async Task<IActionResult> ClassRoom(long id)
        {          
            return View(await _roomService.GetByIdAsync(id));
        }

        public async Task<IActionResult> Dashboard()
        {
            IEnumerable<GetClassRoomItemVM> data = await _roomService.GetAllAsync();

            return View(data);
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
