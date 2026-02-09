using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionClientService _questionService;

        public QuestionController(IQuestionClientService questionClient)
        {
            _questionService = questionClient;
        }
        public async Task<IActionResult> Update(long id, long quizId, long classRoomId, string questionType)
        {
            if (id < 1 || quizId < 1 || classRoomId < 1)
                return BadRequest();


            UpdateQuestionVM? updateQuestion = await _questionService
                                            .GetUpdateAsync(id, quizId, classRoomId, questionType);
            if (updateQuestion is null)
                return NotFound();

            return View(updateQuestion);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateQuestionVM questionVM, long id, long quizId, long classRoomId, string questionType)
        {
            if (id < 1 || quizId < 1 || classRoomId < 1)
                return BadRequest();

            if (questionVM.PutChoice is null && questionVM.PutOpen is null)
                return BadRequest();

            if (!(questionType == "SingleChoice"
                || questionType == "OpenResponce"))
                return BadRequest();

            ServiceResult result = await _questionService.UpdateAsync(id, questionVM, quizId, classRoomId, questionType);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);
                return View(questionVM);
            }

            return RedirectToAction("Update","Quiz",new {classRoomId,
                                                         id=quizId});

        }
    }
}
