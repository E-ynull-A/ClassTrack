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
            return Ok(await _tokenService.RefreshAsync(rToken));
        }

        [HttpPost("Reset")]
        public async Task<IActionResult> Post(ResetTokenDTO tokenDTO)
        {
           await _tokenService.GenerateResetTokenAsync(tokenDTO);
            return Created();
        }
    }
}
