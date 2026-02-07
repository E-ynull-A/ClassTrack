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
    public class StudentAttendancesController : ControllerBase
    {
        private readonly IStudentAttendanceService _attendanceService;

        public StudentAttendancesController(IStudentAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]

        public async Task<IActionResult> Get(int page = 0,int take = 0)
        {     
            return Ok(await _attendanceService.GetAllAsync(page, take));
        }

        [HttpPost]

        public async Task<IActionResult> Post(ICollection<PostStudentAttendanceDTO> attendanceDTOs)
        {
            await _attendanceService
                            .CreateAttendanceAsync(attendanceDTOs);

            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Put(ICollection<PutStudentAttendanceDTO> attendanceDTOs)
        {
            await _attendanceService.UpdateAttendanceAsync(attendanceDTOs);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            if (id < 1)
                return BadRequest();

            await _attendanceService.DeleteAttendanceAsync(id);
            return NoContent();
        }
    }


}
