using ClassTrack.API.ActionFilter;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class QuizesController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizesController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet("{classRoomId?}")]
        [ServiceFilter(typeof(ClassRoomAccessFilter))]
        public async Task<IActionResult> Get(long classRoomId = 0, int page = 0, int take = 0)
        {
            return Ok(await _quizService.GetAllAsync(classRoomId, page, take));
        }

        [HttpGet("{classRoomId?}/{id}")]     
        public async Task<IActionResult> Get(long id)
        {
            if (id < 1)
                return BadRequest();

           return Ok(await _quizService.GetByIdAsync(id));
        }

        [HttpGet("{classRoomId}/{id}/Detail")]
        [ServiceFilter(typeof(ClassRoomAccessFilter))]
        public async Task<IActionResult> GetDetail(long id)
        {
            if (id < 1)
                return BadRequest();

           return Ok(await _quizService.GetByIdDetailAsync(id));
        }

        [HttpPost("{classRoomId}")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Post(PostQuizDTO quizDTO)
        {
            await _quizService.CreateQuizAsync(quizDTO);

            return Created();
        }

        [HttpPut("{classRoomId}/{id}")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Put(long id,PutQuizDTO quizDTO)
        {
            if(id < 1)
                return BadRequest();

            await _quizService.UpdateQuizAsync(id,quizDTO);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id)
        {
            if (id < 1)
                return BadRequest();

            await _quizService.DeleteQuizAsync(id);
            return NoContent();
        }

        [HttpDelete("{classRoomId}/{id}/Restore")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> SoftDelete(long id)
        {
            if(id < 1)
                return BadRequest();

            await _quizService.SoftQuizDeleteAsync(id);
            return NoContent();
        }

    }
}
