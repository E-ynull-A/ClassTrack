using Microsoft.AspNetCore.Mvc;

namespace ClassTrack.MVC.Controllers
{
    public class AttendanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
