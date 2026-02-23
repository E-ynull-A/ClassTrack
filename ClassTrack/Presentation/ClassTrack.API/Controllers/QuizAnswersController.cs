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
    public class QuizAnswersController : ControllerBase
    {
        private readonly IQuizAnswerService _answerService;

        public QuizAnswersController(IQuizAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet("{classRoomId}/{quizId}/{studentId}/Student")]
        [ServiceFilter(typeof(ClassRoomAccessFilter))]
        public async Task<IActionResult> Get(long studentId,long quizId, int page = 1, int take = 5)
        {
            if (studentId < 1 || quizId < 1)
                return BadRequest();

            return Ok(await _answerService
                .GetAllByStudentIdAsync(page: page,
                                        take: take,
                                        studentId: studentId,
                                        quizId:quizId));
        }

        [HttpGet("{classRoomId}/{id}")]
        [ServiceFilter(typeof(ClassRoomAccessFilter))]
        public async Task<IActionResult> Get(long id)
        {
            if(id < 1)
                return BadRequest();

            return Ok(await _answerService.GetByIdAsync(id));
        }

        [HttpPost("{classRoomId}")]
        [ServiceFilter(typeof(ClassRoomAccessFilter))]

        public async Task<IActionResult> Post(PostQuizAnswerDTO answerDTO)
        {
            await _answerService.TakeAnExamAsync(answerDTO);
            return Created();
        }

        [HttpPut("{classRoomId}/{id}/Evaulate")]
        [ServiceFilter(typeof(TeacherAccessFilter))]
        public async Task<IActionResult> Put(long id,PutQuizAnswerDTO answerDTO)
        {
            if(id < 1) return BadRequest();

            await _answerService.EvaluateAnswerAsync(id,answerDTO);
            return NoContent();
        }


    }
}
