using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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


        public async Task<GetQuizAnswerVM?> GetByIdAsync(long classRoomId,long id)
        {
           return await _httpClient
                            .GetFromJsonAsync<GetQuizAnswerVM>
                                    ($"QuizAnswers/{classRoomId}/{id}");
        }
        public async Task<ICollection<GetQuizAnswerItemVM>?> GetAllAsync(long classRoomId,long quizId,long studentId)
        {
           return await _httpClient.GetFromJsonAsync
                            <ICollection<GetQuizAnswerItemVM>>
                                        ($"QuizAnswers/{classRoomId}/{quizId}/{studentId}/Student");
        }
        public async Task<ServiceResult> TakeAnExamAsync(long classRoomId,PostQuizAnswerVM studentAnswer)
        {
            var message = await _httpClient.PostAsJsonAsync($"QuizAnswers/{classRoomId}",studentAnswer);

            if (!message.IsSuccessStatusCode)            
                return new ServiceResult(false, string.Empty, await message.Content.ReadAsStringAsync());

            return new ServiceResult(true);
        }
        public async Task<ServiceResult> EvaulateAsync(PutQuizAnswerVM answerVM,long id,long classRoomId)
        {
            if (answerVM.Point < 0 || answerVM.Point > 100)
                return new ServiceResult(false, nameof(PutQuizAnswerVM.Point), "The Point Value is Invalid");

            HttpResponseMessage message = await _httpClient.PutAsJsonAsync($"QuizAnswers/{classRoomId}/{id}/Evaulate",answerVM);

            if (!message.IsSuccessStatusCode)
                return new ServiceResult(false, string.Empty, await message.Content.ReadAsStringAsync());

            return new ServiceResult(true);
        }
        
    }
}
