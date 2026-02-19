using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Services.Implementations
{
    public class TaskWorkClientService : ITaskWorkClientService
    {
        private readonly HttpClient _httpClient;
        public TaskWorkClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }

        public async Task<ServiceResult> CreateAsync(long classRoomId, PostTaskWorkVM taskWorkVM)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(taskWorkVM.Title), "taskWork.Title");
            content.Add(new StringContent(taskWorkVM.StartDate.ToString("yyyy-MM-dd")), "StartDate");
            content.Add(new StringContent(taskWorkVM.EndDate.ToString("yyyy-MM-dd")), "EndDate");
            content.Add(new StringContent(taskWorkVM.MainPart), "MainPart");
            content.Add(new StringContent(taskWorkVM.ClassRoomId.ToString()), "ClassRoomId");

            if (taskWorkVM.AttachmentVM is not null)
            {
                foreach (IFormFile file in taskWorkVM.AttachmentVM.Files)
                {
                    StreamContent fileContent = new StreamContent(file.OpenReadStream());

                    content.Add(fileContent, "AttachmentVM.Files", file.FileName);
                }
            }

            HttpResponseMessage message = await _httpClient.PostAsJsonAsync($"TaskWorks/{classRoomId}", content);

            if (!message.IsSuccessStatusCode)
            {
                var error = await message.Content.ReadFromJsonAsync<ErrorResponseVM>();
                return new ServiceResult(false, string.Empty, error.Message);
            }

            return new ServiceResult(true);
        }
    }
}
