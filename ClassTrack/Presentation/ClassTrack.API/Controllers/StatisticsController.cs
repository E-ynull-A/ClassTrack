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
    //[Authorize(Roles = "Admin")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        private readonly IAuthenticationService _authenticationService;

        public StatisticsController(IStatisticsService statisticsService,
                                    IAuthenticationService authenticationService)
        {
            _statisticsService = statisticsService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
           return Ok(await _statisticsService.GetCountAsync());
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Get(int page = 1,int take = 15)
        {
           return Ok(await _statisticsService.GetAllUserAsync(page,take));
        }

        [HttpPatch]
        public async Task<IActionResult> Ban(PostBanUserDTO postBan)
        {
           await _authenticationService.BanUserAsync(postBan);
            return NoContent();
        }
    }
}
