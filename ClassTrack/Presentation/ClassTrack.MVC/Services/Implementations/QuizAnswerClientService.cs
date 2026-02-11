using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Implementations
{
    public class QuizAnswerClientService:IQuizAnswerClientService
    {
        private readonly HttpClient _httpClient;
        public QuizAnswerClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }

        public ServiceResult TakeAnExamAsync(long classRoomId)
        {

        }
    }
}
