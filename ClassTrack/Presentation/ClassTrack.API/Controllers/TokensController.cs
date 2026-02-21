using ClassTrack.Application.DTOs;
using ClassTrack.Application.DTOs.Token;
using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokensController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("Refresh/{rToken}")]
        public async Task<IActionResult> Get(string rToken)
        {
            return Ok(await _tokenService.RefreshAsync(rToken, 15));
        }

        [HttpPost("Reset")]
        public async Task<IActionResult> Post(GetEmailForTokenDTO tokenDTO)
        {
            await _tokenService.GenerateResetTokenAsync(tokenDTO);
            return Created();
        }

        [HttpPost("Leave")]
        public async Task<IActionResult> Post(LeaveClassRoomDTO leaveClassRoom)
        {
            await _tokenService.GenerateLeaveTokenAsync(leaveClassRoom);
            return Created();
        }
    }
}
