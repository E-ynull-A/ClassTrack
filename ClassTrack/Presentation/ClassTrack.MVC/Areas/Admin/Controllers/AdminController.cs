using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminClientService _adminClient;
        private readonly IQuizClientService _quizClient;
        private readonly ITaskWorkClientService _taskWorkClient;

        public AdminController(IAdminClientService adminClient,
                                IQuizClientService quizClient,
                                ITaskWorkClientService taskWorkClient)
        {
            _adminClient = adminClient;
            _quizClient = quizClient;
            _taskWorkClient = taskWorkClient;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _adminClient.GetDasboardAsync());
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Users(int page = 1)
        {
            if (page < 1)
                return BadRequest();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = 15;

            return View(new UsersVM(await _adminClient.GetUserAllAsync(page)));
        }

        [HttpPost]
        public async Task<IActionResult> BanUser(UsersVM usersVM)
        {
            if (!ModelState.IsValid)
            {
               return RedirectToAction("Users");
            }

            ServiceResult result = await _adminClient.BanUserAsync(usersVM.PostBan);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);               
            }

            return RedirectToAction("Users");
        }

        [HttpGet("Quizzes")]
        public async Task<IActionResult> Quizzes(int page = 1)
        {
            if (page < 0)
                return BadRequest();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = 8;

            return View(await _adminClient.GetAllAsync(page,3));
        }

        public async Task<IActionResult> DeleteQuiz(long id)
        {
           await _adminClient.DeleteQuizAsync(id);
            return RedirectToAction("Quizzes");
        }
        public async Task<IActionResult> QuizDetail(long id)
        {
            return View(await _quizClient.GetQuizInDetailAsync(id));
        }
        public async Task<IActionResult> TaskWorks(int page = 1)
        {
            if (page < 0)
                return BadRequest();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = 8;


            return View(await _adminClient.GetTaskWorkAllAsync(page,8));
        }
        public async Task<IActionResult> DeleteTaskWork(long id)
        {
            if(id < 1)
                return BadRequest();

            await _adminClient.DeleteTaskAsync(id);
            return RedirectToAction("TaskWorks");
        }

    }
}
