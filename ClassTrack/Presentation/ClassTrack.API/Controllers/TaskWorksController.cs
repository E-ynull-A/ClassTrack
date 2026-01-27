using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskWorksController : ControllerBase
    {
        private readonly ITaskWorkService _taskService;

        public TaskWorksController(ITaskWorkService taskService)
        {
            _taskService = taskService;
        }
    }
}
