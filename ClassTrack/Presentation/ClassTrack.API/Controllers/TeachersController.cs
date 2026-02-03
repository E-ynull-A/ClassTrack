using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeacherClasses(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("The User isn't Found!");

            return Ok(await _teacherService.GetTeacherClassesAsync(userId));
        }
    }
}
