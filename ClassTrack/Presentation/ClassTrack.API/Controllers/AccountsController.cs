using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountsController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            await _authenticationService.RegisterAsync(registerDTO);

            return Created();
        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            return Ok(await _authenticationService.LoginAsync(loginDTO));
        }

        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout()
        {
           await _authenticationService.LogoutAsync();
           return NoContent();
        }

        [HttpPut("Password")]
        public async Task<IActionResult> Put(ResetPasswordDTO resetPassword)
        {
           await _authenticationService.ResetPasswordAsync(resetPassword);
            return NoContent();
        }
    }
}
