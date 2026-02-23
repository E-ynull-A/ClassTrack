using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskWorkAttachmentsController : ControllerBase
    {
        private readonly ITaskWorkService _taskWorkService;

        public TaskWorkAttachmentsController(ITaskWorkService taskWorkService)
        {
            _taskWorkService = taskWorkService;
        }

        [HttpGet("{classWorkId}/{taskWorkId}")]
        public IActionResult Get(long taskWorkId)
        {
            if (taskWorkId < 1)
                return BadRequest();

            return Ok(_taskWorkService.GetAttachmentAsync(taskWorkId));
        }
    }
}
