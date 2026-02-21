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

        [HttpGet("{classRoomId}/Get")]
        public async Task<IActionResult> Get(long classRoomId,int page = 1)
        {
            if (classRoomId < 1)
                return BadRequest();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = 6;

          return View("Index",await _taskWorkClient.GetAllAsync(page,6,classRoomId));
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

            return RedirectToAction("ClassRoom","Class",new {id=classRoomId});
        }

        [HttpGet("{classRoomId}/{id}/TaskWork")]
        public IActionResult Update(long classRoomId,long id)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Update(long classRoomId,long id,PutTaskWorkVM putTask)
        {
            if(classRoomId < 1 || id < 1)
                return BadRequest();

            ServiceResult result = await _taskWorkClient.UpdateAsync(id, classRoomId, putTask);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);
                return View(putTask);
            }

            return RedirectToAction(nameof(Get), new {classRoomId});
        }
    }
}
