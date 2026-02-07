using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizClientService _quizClient;

        public QuizController(IQuizClientService quizClient)
        {
            _quizClient = quizClient;
        }
        public async Task<IActionResult> Index(long id)
        {         
            return View(await _quizClient.GetAllAsync(id));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostQuizVM postQuiz)
        {
            if(!ModelState.IsValid)
                return View(postQuiz);

            ServiceResult result = await _quizClient.CreateQuizAsync(postQuiz);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);
                return View(postQuiz);
            }

            return RedirectToAction("ClassRoom", "Class",new {id = postQuiz.ClassRoomId });
        }

        
        public IActionResult Put(long id)
        {

        }

        
    }
}
