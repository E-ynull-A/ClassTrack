using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Controllers
{
    public class QuizAnswerController : Controller
    {
        private readonly IQuizClientService _quizClient;
        private readonly IQuizAnswerClientService _quizAnswer;

        public QuizAnswerController(IQuizClientService quizClient,
                                    IQuizAnswerClientService quizAnswer)
        {
            _quizClient = quizClient;
            _quizAnswer = quizAnswer;
        }


        [HttpGet("ClassRoom/{classRoomId}/Quiz/{quizId}/Student/{studentId}")]
        public async Task<IActionResult> Index(long classRoomId, long quizId, long studentId)
        {
            if (classRoomId < 1 || quizId < 1 || studentId < 1)
                return BadRequest();

           return View("QuizAnswerList",
                        await _quizAnswer.GetAllAsync  
                                (classRoomId,quizId,studentId));
        }

        [HttpPost]
        public async Task<IActionResult> Post(ExamVM examVM,long classRoomId,long quizId)
        {
            if (classRoomId < 1)
                return BadRequest();
         
            if (!ModelState.IsValid)
            {
                return View("Exam",new ExamVM(await _quizClient
                            .GetQuizForStudentAsync(classRoomId, quizId),examVM.QuizAnswer));
            }

            ServiceResult result = await _quizAnswer.TakeAnExamAsync(classRoomId,examVM.QuizAnswer);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);
                return View("Exam", new ExamVM(await _quizClient.GetQuizForStudentAsync(classRoomId, quizId), examVM.QuizAnswer));
            }

            return RedirectToAction("Index","Quiz",new {id = classRoomId});
        }

        [HttpGet("ClassRoom/{classRoomId}/Quiz/{quizId}/Student/{studentId}/QuizAnswer/{id}")]
        public async Task<IActionResult> GetById(long classRoomId,long id,long quizId,long studentId)
        {
            if (classRoomId < 1 || id < 1 || quizId < 1 || studentId < 1)
                return BadRequest();

           return View("QuizAnswer",new EvaulateVM(await _quizAnswer
                                .GetByIdAsync(classRoomId, id)));
        }

        [HttpPost("ClassRoom/{classRoomId}/Quiz/{quizId}/Student/{studentId}/QuizAnswer/{id}")]
        public async Task<IActionResult> Evaulate(long classRoomId,
                                                  long quizId,
                                                  long studentId,
                                                  long id,
                                                  EvaulateVM answerVM)
        {
            if (classRoomId < 1 || id < 1 || quizId < 1 || studentId < 1)
                return BadRequest();

            ServiceResult result = await _quizAnswer.EvaulateAsync(answerVM.PutQuizVM, id, classRoomId);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey,result.ErrorMessage);
                return View("QuizAnswer",new EvaulateVM(await _quizAnswer.GetByIdAsync(classRoomId,id)));
            }

            return RedirectToAction(nameof(Index),new {classRoomId,quizId,studentId});
        }

    }
}
