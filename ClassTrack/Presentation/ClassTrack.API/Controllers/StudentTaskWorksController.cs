using ClassTrack.API.ActionFilter;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentTaskWorksController : ControllerBase
    {
        private readonly ITaskWorkService _taskWorkService;

        public StudentTaskWorksController(ITaskWorkService taskWorkService)
        {
            _taskWorkService = taskWorkService;
        }



        [HttpGet("{classRoomId}/{taskWorkId}/Evaulate")]
        public async Task<IActionResult> Get(long taskWorkId)
        {
            if (taskWorkId < 1)
                return BadRequest();

            return Ok(await _taskWorkService.GetStudentTaskWorkAsync(taskWorkId));
        }

        [HttpPut("{classRoomId}/{taskWorkId}/Evaulate")]
        public async Task<IActionResult> Put(PutPointInTaskWorkDTO putPoint, long taskWorkId)
        {
            if (taskWorkId < 1)
                return BadRequest();

            await _taskWorkService.EvaulateTaskAsync(putPoint, taskWorkId);
            return NoContent();
        }
    }
}
