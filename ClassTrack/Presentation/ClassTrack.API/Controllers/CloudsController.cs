using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace ClassTrack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudsController : ControllerBase
    {
        private readonly IFileService _cloudService;

        public CloudsController(IFileService cloudService)
        {
            _cloudService = cloudService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            await _cloudService.UploadAsync(file);
            return Created();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string publicId)
        {
            string rst = await _cloudService.DeleteFileAsync(publicId);                  
            Console.WriteLine(rst);

            return NoContent();
        }
    }
}
