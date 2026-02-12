using ClassTrack.API.ActionFilter;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _service;

        public QuestionsController(IQuestionService service)
        {
            _service = service;
        }



        [HttpGet("{classRoomId}/{quizId}")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Get(long quizId,int page = 0,int take = 0)
        {
           return Ok(await _service.GetAllAsync(quizId,page:page,take:take));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            if (id < 1)
                return BadRequest();

            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("Choice/{classRoomId}/{quizId}/{id}")]
        public async Task<IActionResult> GetInUpdateChoice(long id)
        {
           if(id < 1)
               return BadRequest();

           return Ok(await _service.GetChoiceForUpdateAsync(id));
        }

        [HttpGet("Open/{classRoomId}/{quizId}/{id}")]

        public async Task<IActionResult> GetInUpdateOpen(long id)
        {

            if (id < 1)
                return BadRequest();

            return Ok(await _service.GetOpenForUpdateAsync(id));
        }
        

        [HttpPost("{classRoomId}/{quizId}/ChoiceQuestion")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Post(PostChoiceQuestionDTO postChoice)
        {
            if(postChoice.Options is null)
             {return BadRequest(); }

            await _service.CreateChoiceQuestionAsync(postChoice);
            return Created();
        }

        [HttpPost("{classRoomId}/{quizId}/OpenQuestion")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Post(PostOpenQuestionDTO postOpen)
        {
            if(postOpen is null)
                return BadRequest();

            await _service.CreateOpenQuestionAsync(postOpen);
            return Created();
        }

        [HttpPut("{classRoomId}/{quizId}/{id}/ChoiceQuestion")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Put(long id,long classRoomId,PutChoiceQuestionDTO putChoice, long quizId)
        {
            if(id < 1)
                return BadRequest();

            await _service.UpdateChoiceQuestionAsync(id,putChoice,quizId);
            return NoContent();
        }

        [HttpPut("{classRoomId}/{quizId}/{id}/OpenQuestion")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Put(long id,long classRoomId,PutOpenQuestionDTO putOpen, long quizId)
        {
            if(id<1)
                return BadRequest();

            await _service.UpdateOpenQuestionAsync(id,putOpen, quizId);
            return NoContent();
        }

        [HttpDelete("{id}/{classRoomId}/ChoiceQuestion")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> DeleteChoice(long id)
        {
            if(id<1)
                return BadRequest();

            await _service.DeleteChoiceQuestionAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}/{classRoomId}/OpenQuestion")]
        [ServiceFilter(typeof(TeacherAccessFilter))]

        public async Task<IActionResult> DeleteOpen(long id)
        {
            if (id < 1)
                return BadRequest();

            await _service.DeleteOpenQuestionAsync(id);
            return NoContent();
        }

    }
}
