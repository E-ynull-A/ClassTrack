using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
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
        public async Task<IActionResult> Get(long classRoomId, int page = 1)
        {
            if (classRoomId < 1)
                return BadRequest();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = 6;

            return View("Index", await _taskWorkClient.GetAllAsync(page, 6, classRoomId));
        }

        [HttpGet("{classRoomId}/TaskWork")]
        public IActionResult Post(long classRoomId)
        {
            return View("Create-Task");
        }

        [HttpPost("{classRoomId}/TaskWork")]
        public async Task<IActionResult> Post(long classRoomId, PostTaskWorkVM postTask)
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

            return RedirectToAction("ClassRoom", "Class", new { id = classRoomId });
        }

        public async Task<IActionResult> Update(long classRoomId, long id)
        {
            if (classRoomId < 1 || id < 1)
                return BadRequest();

            GetTaskWorkVM? taskWorkVM = await _taskWorkClient.GetByIdAsync(classRoomId, id);
            if (taskWorkVM == null)
                throw new Exception("The TaskWork Not Found!");

            ICollection<GetTaskWorkAttachmentVM> taskWorkAttachs = taskWorkVM
                .TaskWorkAttachments.Select(twa => new GetTaskWorkAttachmentVM(
                    twa.Id,
                    twa.FileUrl,
                    twa.FileType,
                    twa.FileName)).ToImmutableList();

            PutTaskWorkVM putTask = new PutTaskWorkVM(

                taskWorkVM.Title,
                taskWorkVM.MainPart,
                taskWorkVM.EndDate,
                taskWorkVM.StartDate);

            return View(new UpdateVM(taskWorkAttachs,putTask));
        }

        [HttpPost]
        public async Task<IActionResult> Update(long classRoomId, long id, UpdateVM updateVM)
        {
            if (classRoomId < 1 || id < 1)
                return BadRequest();

            ServiceResult result = await _taskWorkClient.UpdateAsync(id, classRoomId, updateVM.PutTaskWorkVM);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);
                return View(new UpdateVM(await _taskWorkClient.GetAllTaskAttachmentAsync(classRoomId,id),updateVM.PutTaskWorkVM));
            }

            return RedirectToAction(nameof(Get), new { classRoomId });
        }

        public async Task<IActionResult> Submit(long classRoomId,long taskWorkId)
        {
            if (classRoomId < 1 || taskWorkId < 1)
                return BadRequest();

            return View(new SubmitVM
                    (await _taskWorkClient
                        .GetByIdAsync(classRoomId, taskWorkId)));
        }

        [HttpPost]
        public async Task<IActionResult> Submit(long classRoomId,long taskWorkId,SubmitVM submitVM)
        {
            if(classRoomId < 1 || taskWorkId < 1)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(new SubmitVM
                    (await _taskWorkClient
                        .GetByIdAsync(classRoomId, taskWorkId),submitVM.PutStudentTask));

            await _taskWorkClient.StudentSubmitAsync(classRoomId,taskWorkId,submitVM.PutStudentTask);
            return RedirectToAction(nameof(Get), new { classRoomId});
        }

        public async Task<IActionResult> Evaulate(long classRoomId,long taskWorkId)
        {
            if(classRoomId < 1 && taskWorkId < 1)
                return BadRequest();

            return View(new TaskEvaulateVM(               
               await _taskWorkClient.GetByIdAsync(classRoomId,taskWorkId),
               await _taskWorkClient.GetStudentAnswerAsync(taskWorkId,classRoomId)));
        }


        [HttpPost]
        public async Task<IActionResult> Evaulate(long classRoomId,long taskWorkId,PutPointInTaskWorkVM putPointVM)
        {
            if (classRoomId < 1 && taskWorkId < 1)
                return BadRequest();

            if(!ModelState.IsValid)
                return View(new TaskEvaulateVM(
               await _taskWorkClient.GetByIdAsync(classRoomId, taskWorkId),
               await _taskWorkClient.GetStudentAnswerAsync(taskWorkId, classRoomId)));

            await _taskWorkClient.EvaulateAsync(classRoomId, taskWorkId, putPointVM);

            return RedirectToAction("Get", new { classRoomId });
        }

 
    }
}
