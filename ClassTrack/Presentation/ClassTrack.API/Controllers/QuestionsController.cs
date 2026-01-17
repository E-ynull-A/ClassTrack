using ClassTrack.Application.Interfaces.Services;
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
        public async Task<IActionResult> Get(int page,int take)
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


    }
}
