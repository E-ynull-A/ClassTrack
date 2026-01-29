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
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _service;

        public QuestionsController(IQuestionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 0,int take = 0)
        {
           return Ok(await _service.GetAllAsync(page:page,take:take));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            if (id < 1)
                return BadRequest();

            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost("/ChoiceQuestion")]
        public async Task<IActionResult> Post([FromBody]PostChoiceQuestionDTO postChoice)
        {
            if(postChoice.Options is null)
            { return BadRequest(); }

            await _service.CreateChoiceQuestionAsync(postChoice);
            return Created();
        }

        [HttpPost("/OpenQuestion")]
        public async Task<IActionResult> Post([FromBody]PostOpenQuestionDTO postOpen)
        {
            if(postOpen is null)
                return BadRequest();

            await _service.CreateOpenQuestionAsync(postOpen);
            return Created();
        }

        [HttpPut("/ChoiceQuestion")]
        public async Task<IActionResult> Put(long id,PutChoiceQuestionDTO putChoice)
        {
            if(id < 1)
                return BadRequest();

            await _service.UpdateChoiceQuestionAsync(id,putChoice);
            return NoContent();
        }

        [HttpPut("/OpenQuestion")]
        public async Task<IActionResult> Put(long id,PutOpenQuestionDTO putOpen)
        {
            if(id<1)
                return BadRequest();

            await _service.UpdateOpenQuestionAsync(id,putOpen);
            return NoContent();
        }

        [HttpDelete("{id}/ChoiceQuestion")]
        public async Task<IActionResult> DeleteChoice(long id)
        {
            if(id<1)
                return BadRequest();

            await _service.DeleteChoiceQuestionAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}/OpenQuestion")]

        public async Task<IActionResult> DeleteOpen(long id)
        {
            if (id < 1)
                return BadRequest();

            await _service.DeleteOpenQuestionAsync(id);
            return NoContent();
        }

    }
}
