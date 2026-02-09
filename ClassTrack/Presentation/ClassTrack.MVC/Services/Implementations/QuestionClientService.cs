using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Services.Implementations
{
    public class QuestionClientService : IQuestionClientService
    {
        private readonly HttpClient _httpClient;
        public QuestionClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }

        public async Task<ICollection<GetQuestionItemVM>> GetAllAsync(long classRoomId, long quizId)
        {
            ICollection<GetQuestionItemVM>? questions = await _httpClient.GetFromJsonAsync
                            <ICollection<GetQuestionItemVM>>($"Questions/{classRoomId}/{quizId}");

            if (questions is null)
                return new List<GetQuestionItemVM>();

            return questions;
        }

        public async Task<GetChoiceQuestionForUpdateVM?> GetChoiceForUpdateAsync(long classRoomId, long quizId, long id)
        {
            return await _httpClient.GetFromJsonAsync<GetChoiceQuestionForUpdateVM>
                            ($"Questions/Choice/{classRoomId}/{quizId}/{id}");
        }

        public async Task<GetOpenQuestionForUpdateVM?> GetOpenForUpdateAsync(long classRoomId, long quizId, long id)
        {
            return await _httpClient.GetFromJsonAsync<GetOpenQuestionForUpdateVM>
                            ($"Questions/Open/{classRoomId}/{quizId}/{id}");
        }


        public async Task<UpdateQuestionVM> GetUpdateAsync(long id, long quizId, long classRoomId, string questionType)
        {
            if (questionType == "SingleChoice")
            {
                GetChoiceQuestionForUpdateVM? question = await GetChoiceForUpdateAsync(classRoomId, quizId, id);
                if (question is null)
                    throw new Exception("The Question not Found!");

                IList<PutOptionInChoiceQuestionVM> options = question.Options
                        .Select(o => new PutOptionInChoiceQuestionVM(o.Id, o.Variant, o.IsCorrect,o.IsDeleted)).ToList();

                return new UpdateQuestionVM
                                (new PutChoiceQuestionVM
                                        (question.Title, question.Point, question.IsMultiple, options), null);
            }

            else if (questionType == "OpenQuestion")
            {
                GetOpenQuestionForUpdateVM? question = await GetOpenForUpdateAsync(classRoomId, quizId, id);
                if (question is null)
                    throw new Exception("The Question not Found!");

                return new UpdateQuestionVM(null, new PutOpenQuestionVM(question.Title, question.Point));
            }

            return null;
        }

        public async Task<ServiceResult> UpdateAsync(long id, UpdateQuestionVM questionVM, long quizId, long classRoomId, string questionType)
        {

            if (string.IsNullOrWhiteSpace(questionVM.PutChoice.Title))
                return new ServiceResult(false, nameof(PutChoiceQuestionVM.Title), "The Title must be not Empty");

            if (questionVM.PutChoice.Point < 0 || questionVM.PutChoice.Point > 100)
                return new ServiceResult(false, nameof(PutChoiceQuestionVM.Point), "The Point must be between 0 and 100");

            if (questionVM.PutChoice is not null)
            {           
                ICollection<PutOptionInChoiceQuestionVM> options = questionVM.PutChoice.Options;

                if (options.Any(o => o.Id.HasValue && o.Id < 1))
                    return new ServiceResult(false, nameof(PutChoiceQuestionVM.Options), "Bad Option Request");
                if (options is null)
                    return new ServiceResult(false, nameof(PutChoiceQuestionVM.Options), "The Options must be not Empty");
                if (options.Count <= 2 && options.Count > 6)
                    return new ServiceResult(false, nameof(PutChoiceQuestionVM.Options), "The Options Count:min:2 max:5");
                if (options.DistinctBy(o => o.Variant.Trim()).Count() != options.Count)
                    return new ServiceResult(false, nameof(PutChoiceQuestionVM.Options), "The Option's must not be Dublicate");
                if (!questionVM.PutChoice.IsMultiple && options.Count(o => o.IsCorrect) > 1)
                    return new ServiceResult(false, nameof(PutChoiceQuestionVM.Options), "You can choose only one correct Variant");

            }

            HttpResponseMessage message = default;
            if (questionType == "SingleChoice")            
                message = await _httpClient.PutAsJsonAsync($"Questions/{classRoomId}/{quizId}/{id}/ChoiceQuestion", questionVM.PutChoice);
            
            else if (questionType == "OpenResponce")         
                message = await _httpClient.PutAsJsonAsync($"Questions/{classRoomId}/{quizId}/{id}/OpenQuestion", questionVM.PutOpen);
            
            else
                return new ServiceResult(false, string.Empty, "Bad Type Request");

            if (!message.IsSuccessStatusCode)
            {
                return new ServiceResult(false, string.Empty, await message.Content.ReadAsStringAsync());
            }
            return new ServiceResult(true);


           
        }

    }
}
