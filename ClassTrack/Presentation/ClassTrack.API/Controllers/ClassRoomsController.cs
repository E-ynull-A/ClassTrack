using ClassTrack.API.ActionFilter;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    
    public class ClassRoomsController : ControllerBase
    {
        private readonly IClassRoomService _roomService;

        public ClassRoomsController(IClassRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 0,int take = 0)
        {
            return Ok(await _roomService.GetAllAsync(page:page,take:take));
        }

        [HttpGet("{classRoomId}")]
        [ServiceFilter(typeof(ClassRoomAccessFilter))]
        public async Task<IActionResult> Get(long classRoomId)
        {
            if (classRoomId < 1)
                return BadRequest();

            return Ok(await _roomService.GetByIdAsync(classRoomId));
        }
        [HttpPost]
        public async Task<IActionResult> Post(PostClassRoomDTO roomDTO)
        {
            await _roomService.CreateClassRoomAsync(roomDTO);

            return Created();
        }

        [HttpPut("{classRoomId}")]
        [ServiceFilter(typeof(ClassRoomAccessFilter))]
        public async Task<IActionResult> Put(long classRoomId,PutClassRoomDTO roomDTO)
        {
            if (classRoomId < 1)
                return BadRequest();

            await _roomService.UpdateClassRoomAsync(classRoomId, roomDTO);
            return NoContent();
        }

        [HttpDelete("{classRoomId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long classRoomId)
        {
            if (classRoomId < 1)
                return BadRequest();

            await _roomService.DeleteClassRoomAsync(classRoomId);
            return NoContent();
        }

        [HttpDelete("{classRoomId}/Restore")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> SoftDelete(long classRoomId)
        {
            await _roomService.SoftDeleteClassRoomAsync(classRoomId);
            return NoContent();
        }



    }
}
