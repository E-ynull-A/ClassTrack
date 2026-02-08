using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Services.Implementations
{
    public class QuestionClientService:IQuestionClientService
    {
        private readonly HttpClient _httpClient;
        public QuestionClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }

        public async Task<ICollection<GetQuestionItemVM>> GetAllAsync(long classRoomId,long quizId)
        {
            ICollection<GetQuestionItemVM>? questions = await _httpClient.GetFromJsonAsync
                            <ICollection<GetQuestionItemVM>>($"Questions/{classRoomId}/{quizId}");

            if (questions is null)
                return new List<GetQuestionItemVM>();

            return questions;
        }

        
    }
}
