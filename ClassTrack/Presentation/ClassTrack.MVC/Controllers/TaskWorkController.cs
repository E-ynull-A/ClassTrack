using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Controllers
{
    public class TaskWorkController : Controller
    {
        private readonly ITaskWorkClientService _taskWorkClient;

        public TaskWorkController(ITaskWorkClientService taskWorkClient)
        {
            _taskWorkClient = taskWorkClient;
        }


        [HttpGet("{classRoomId}/TaskWork")]
        public IActionResult Post(long classRoomId)
        {
            return View("Create-Task");
        }

        [HttpPost("{classRoomId}/TaskWork")]
        public async Task<IActionResult> Post(long classRoomId,PostTaskWorkVM postTask)
        {
            if (!ModelState.IsValid)
            {
                return View("Create-Task", postTask);
            }

            ServiceResult result = await _taskWorkClient.CreateAsync(classRoomId, postTask);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);
                return View("Create-Task", postTask);
            }

            return RedirectToAction("GetByIdAsync","Class",new {classRoomId});
        }
    }
}
