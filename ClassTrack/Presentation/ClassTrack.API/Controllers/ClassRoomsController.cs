using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            if (id < 1)
                return BadRequest();

            return Ok(await _roomService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(PostClassRoomDTO roomDTO)
        {
            await _roomService.CreateClassRoomAsync(roomDTO);

            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Put(long id,PutClassRoomDTO roomDTO)
        {
            if (id < 1)
                return BadRequest();

            await _roomService.UpdateClassRoomAsync(id, roomDTO);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            if (id < 1)
                return BadRequest();

            await _roomService.DeleteClassRoomAsync(id);
            return NoContent();
        }

    }
}
