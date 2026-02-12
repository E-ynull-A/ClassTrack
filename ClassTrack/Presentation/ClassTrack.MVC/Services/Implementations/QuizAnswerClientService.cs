using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Services.Implementations
{
    public class QuizAnswerClientService:IQuizAnswerClientService
    {
        private readonly HttpClient _httpClient;
        public QuizAnswerClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }

        public async Task<ServiceResult> TakeAnExamAsync(long classRoomId,PostQuizAnswerVM studentAnswer)
        {
            var message = await _httpClient.PostAsJsonAsync($"QuizAnswers/{classRoomId}",studentAnswer);

            if (!message.IsSuccessStatusCode)            
                return new ServiceResult(false, string.Empty, await message.Content.ReadAsStringAsync());

            return new ServiceResult(true);
        }
    }
}
