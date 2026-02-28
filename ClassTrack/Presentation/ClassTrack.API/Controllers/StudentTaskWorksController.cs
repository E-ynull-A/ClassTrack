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


        [HttpGet("{classRoomId}/{taskWorkId}/{studentId}")]
        public async Task<IActionResult> Get(long taskWorkId,long studentId)
        {
            if (taskWorkId < 1 || studentId < 1)
                return BadRequest();

            return Ok(await _taskWorkService.GetStudentTaskWorkAsync(taskWorkId,studentId));
        }

        [HttpPut("{classRoomId}/{taskWorkId}/{studentId}")]
        public async Task<IActionResult> Put(PutPointInTaskWorkDTO putPoint, long taskWorkId, long studentId)
        {
            if (taskWorkId < 1 || studentId < 1)
                return BadRequest();

            await _taskWorkService.EvaulateTaskAsync(putPoint, taskWorkId,studentId);
            return NoContent();
        }
    }
}
