using ClassTrack.API.ActionFilter;
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

        [HttpGet("{classRoomId}")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Get(long classRoomId, int take = 0,int page = 0)
        {
            if (classRoomId < 0)
                return BadRequest();

            return Ok(await _teacherService.GetAllAsync(classRoomId,page,take));
        }
    }
}
