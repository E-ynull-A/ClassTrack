using Microsoft.AspNetCore.Mvc;

namespace ClassTrack.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        public HomeController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("ClassTrackClient");
        }
        public IActionResult Index()
        {         
            return View();
        }
         
        public IActionResult Login()
        {

            return View();
        }

    }
}
