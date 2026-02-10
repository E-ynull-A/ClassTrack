using ClassTrack.API.ActionFilter;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }


        [HttpGet("{classRoomId}")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Get(long classRoomId,int page = 0, int take = 0)
        {
            if (classRoomId < 0)
                return BadRequest();

            return Ok(await _studentService.GetAllAsync(classRoomId,page,take));
        }

        [HttpPost]
        public async Task<IActionResult> Post(JoinClassRoomDTO joinClassRoomDTO)
        {
            await _studentService.JoinClassAsync(joinClassRoomDTO);

            return Created();
        }

        [HttpDelete("{classRoomId}/{studentId}")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Delete(long classRoomId,long studentId)
        {
            if(classRoomId < 1 || studentId < 1)
                return BadRequest();

            await _studentService.KickAsync(studentId,classRoomId);
            return NoContent();
        }

        [HttpPut]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Put(long classRoomId, long studentId)
        {
            if (classRoomId < 1 || studentId < 1)
                return BadRequest();

            await _studentService.PromoteAsync(studentId,classRoomId);
            return NoContent();
        }



    }
}
