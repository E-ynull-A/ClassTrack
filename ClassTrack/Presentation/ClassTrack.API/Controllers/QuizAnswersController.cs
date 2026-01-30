using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuizAnswersController : ControllerBase
    {
        private readonly IQuizAnswerService _answerService;

        public QuizAnswersController(IQuizAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet("{studentId}/Student")]

        public async Task<IActionResult> Get(long studentId,int page = 0, int take = 0)
        {
            if (studentId < 1)
                return BadRequest();

            return Ok(await _answerService
                .GetAllByStudentIdAsync(page:page,
                                       take:take,
                                       studentId:studentId));
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> Get(long id)
        {
            if(id < 1)
                return BadRequest();

            return Ok(await _answerService.GetByIdAsync(id));
        }

        [HttpPost]

        public async Task<IActionResult> Post(PostQuizAnswerDTO answerDTO)
        {
            await _answerService.TakeAnExamAsync(answerDTO);

            return Created();
        }






    }
}
