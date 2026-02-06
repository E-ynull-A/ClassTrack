using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Implementations
{
    public class QuizClientService : IQuizClientService
    {
        private readonly HttpClient _httpClient;
        public QuizClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }
        public async Task<IEnumerable<GetQuizItemVM>> GetAllAsync(long id)
        {
           return await _httpClient.GetFromJsonAsync<IEnumerable<GetQuizItemVM>>("Quizes?classRoomId=1");
        }

        //public void CreateQuizAsync(PostQuizVM quizVM)
        //{
            


        //}
    }
}
