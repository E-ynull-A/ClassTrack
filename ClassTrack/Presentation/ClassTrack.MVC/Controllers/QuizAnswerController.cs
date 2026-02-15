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



        [HttpPost]
        public async Task<IActionResult> Post(long classRoomId,long quizId,ExamVM examVM)
        {
            if (classRoomId < 1)
                return BadRequest();
         
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Exam",new ExamVM(await _quizClient
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


    }
}
