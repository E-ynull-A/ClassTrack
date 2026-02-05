using Microsoft.AspNetCore.Mvc;

namespace ClassTrack.MVC.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(string message)
        {
            return View(model:message);
        }
    }
}
