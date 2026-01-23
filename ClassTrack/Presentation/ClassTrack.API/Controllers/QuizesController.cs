using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuizesController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizesController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 0,int take = 0)
        {
            return Ok(await _quizService.GetAllAsync(page, take));
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> Get(long id)
        {
            if (id < 1)
                return BadRequest();

           return Ok(await _quizService.GetByIdAsync(id));
        }
    }
}
