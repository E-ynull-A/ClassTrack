using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizClientService _clientService;

        public QuizController(IQuizClientService clientService)
        {
            _clientService = clientService;
        }
        public async Task<IActionResult> Index(long id)
        {         
            return View(await _clientService.GetAllAsync(id));
        }

        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Create(PostQuizVM postQuiz)
        //{
            
        //}
    }
}
