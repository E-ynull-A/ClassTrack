using ClassTrack.API.ActionFilter;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
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

        public async Task<IActionResult> Get(long classRoomId,int take = 0,int page = 0)
        {
            if (classRoomId < 1)
                return BadRequest();

            return Ok(await _taskService.GetAllByClassRoomIdAsync(page:page, take:take, classRoomId: classRoomId));
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> Get(long id)
        {
            if (id < 0)
                return BadRequest();

            return Ok(await _taskService.GetByIdAsync(id));
        }


        [HttpPost("{classRoomId}")]
        //[ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Post([FromForm]PostTaskWorkDTO postTask,long classRoomId)
        {
            await _taskService.CreateTaskWorkAsync(postTask);
            return Created();
        }

        [HttpPut]

        public async Task<IActionResult> Put(long id,PutTaskWorkDTO putTask)
        {
            if(id < 1)
                return BadRequest();

            await _taskService.UpdateTaskWorkAsync(id, putTask);
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
