using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Services.Implementations
{
    public class TaskWorkClientService:ITaskWorkClientService
    {
        private readonly HttpClient _httpClient;
        public TaskWorkClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }

        public async Task<ServiceResult> CreateAsync(long classRoomId,PostTaskWorkVM taskWorkVM)
        {
            HttpResponseMessage message = await _httpClient.PostAsJsonAsync($"TaskWorks/{classRoomId}", taskWorkVM);

            if (!message.IsSuccessStatusCode)
                return new ServiceResult(false, string.Empty,await message.Content.ReadAsStringAsync());

            return new ServiceResult(true);
        }
    }
}
