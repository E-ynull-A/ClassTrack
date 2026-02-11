using AspNetCoreGeneratedDocument;
using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizClientService _quizClient;
        private readonly IQuestionClientService _questionClient;

        public QuizController(IQuizClientService quizClient,
                              IQuestionClientService questionClient)
        {
            _quizClient = quizClient;
            _questionClient = questionClient;
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


        public async Task<IActionResult> Update(long id,long classRoomId)
        {
            if (id < 1)
                return BadRequest();

            GetQuizItemVM getQuizItem = await _quizClient.GetByIdAsync(id,classRoomId);

            return View(new UpdateQuizVM(
                        new PutQuizVM(getQuizItem.Name, getQuizItem.StartTime,
                        getQuizItem.Duration.TotalMinutes, classRoomId),
                        await _questionClient.GetAllAsync(classRoomId, id)));

        }

        [HttpPost]
        public async Task<IActionResult> Update(long id,UpdateQuizVM quizVM)
        {         

            if (!ModelState.IsValid)           
                return View(quizVM);

            ServiceResult serviceResult = await _quizClient.UpdateQuizAsync(quizVM.PutQuiz, id);
            if (!serviceResult.Ok)
            {               
                ModelState.AddModelError(serviceResult.ErrorKey,serviceResult.ErrorMessage);
                return View(new UpdateQuizVM(quizVM.PutQuiz,
                            await _questionClient.GetAllAsync(quizVM.PutQuiz.ClassRoomId, id)));
            }

            return RedirectToAction("Index",new {id = quizVM.PutQuiz.ClassRoomId });           
        }
    
        public async Task<IActionResult> Get(long classRoomId,long id)
        {
            if (classRoomId < 1 || id < 1)
                return BadRequest();

            return View("Exam",await _quizClient
                            .GetQuizForStudentAsync(classRoomId,id));
        }
    }
}
