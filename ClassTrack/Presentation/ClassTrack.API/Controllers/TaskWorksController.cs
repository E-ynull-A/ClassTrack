using ClassTrack.API.ActionFilter;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TaskWorksController : ControllerBase
    {
        private readonly ITaskWorkService _taskService;

        public TaskWorksController(ITaskWorkService taskService)
        {            
                _taskService = taskService;             
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page,int take)
        {
           return Ok(await _taskService.GetAllAsync(page:page,take:take));
        }

        [HttpGet("{classRoomId}/ClassRoom")]
        public async Task<IActionResult> Get(long classRoomId,int page = 0,int take = 0)
        {
            if (classRoomId < 1)
                return BadRequest();

            return Ok(await _taskService.GetAllByClassRoomIdAsync(page:page, take:take, classRoomId: classRoomId));
        }

        [HttpGet("{classRoomId}/{id}/Detail")]
        [ServiceFilter(typeof(ClassRoomAccessFilter))]
        public async Task<IActionResult> GetAnswers(long id)
        {
            if (id < 0)
                return BadRequest();

            return Ok(await _taskService.GetByIdAsync(id));
        }

        [HttpPost("{classRoomId}")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Post([FromForm]PostTaskWorkDTO postTask,long classRoomId)
        {
            if(classRoomId < 1)
                return BadRequest();

            await _taskService.CreateTaskWorkAsync(postTask);
            return Created();
        }

        [HttpPut("{classRoomId}/{id}/Edit")]
        public async Task<IActionResult> Put([FromForm]PutTaskWorkDTO putTask,long id,long classRoomId )
        {
            if(id < 1 || classRoomId < 1)
                return BadRequest();

            await _taskService.UpdateTaskWorkAsync(id,classRoomId, putTask);
            return NoContent();
        }

        [HttpPut("{classRoomId}/{taskWorkId}/Submit")]
        public async Task<IActionResult> Put(PutStudentTaskWorkDTO studentSubmit,long taskWorkId)
        {
            if(taskWorkId < 1)
                return BadRequest();

            await _taskService.StudentSubmitAsync(studentSubmit,taskWorkId);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            if (id < 1)
                return BadRequest();

            await _taskService.DeleteTaskWorkAsync(id);
            return NoContent();
        }
    }
}
