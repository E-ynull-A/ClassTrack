using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Services.Implementations
{
    public class QuizClientService : IQuizClientService
    {
        private readonly HttpClient _httpClient;
        public QuizClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }
        public async Task<GetQuizItemWithPermissionVM> GetAllAsync(long classRoomId, int page, int take)
        {

            return new GetQuizItemWithPermissionVM(await _httpClient.GetFromJsonAsync<GetQuizItemPagedVM>($"Quizes/{classRoomId}?page={page}&take={take}"),
                                                   await _httpClient.GetFromJsonAsync<IsTeacherVM>($"Permissions/{classRoomId}"));

        }
        public async Task<GetQuizItemVM> GetByIdAsync(long id,long classRoomId)
        {    
            return await _httpClient.GetFromJsonAsync<GetQuizItemVM>($"Quizes/{classRoomId}/{id}");
        }
        public async Task<GetQuizVM?> GetQuizForStudentAsync(long classRoomId,long quizId)
        {
           var responce = await _httpClient.GetAsync($"Quizes/{classRoomId}/{quizId}/Detail");

            if (!responce.IsSuccessStatusCode)
            {
                throw new Exception((await responce.Content.ReadFromJsonAsync<ErrorResponseVM>()).Message);
            }

            return await responce.Content.ReadFromJsonAsync<GetQuizVM>();
        }
        public async Task<ServiceResult> CreateQuizAsync(PostQuizVM quizVM)
        {

            if (quizVM.ChoiceQuestions == null && quizVM.OpenQuestions == null)
                return new ServiceResult(false, nameof(PostChoiceQuestionVM), "The Question must not be Empty");

            int totalCount = 0;
            if (quizVM.ChoiceQuestions is not null)
                totalCount += quizVM.ChoiceQuestions.Count;
            if (quizVM.OpenQuestions is not null)
                totalCount += quizVM.OpenQuestions.Count;

            if (totalCount > 200)
            {
                return new ServiceResult(false,string.Empty, "The Count of Quiz is so high");
            }

            if (quizVM.ClassRoomId < 1)
                return new ServiceResult(false,string.Empty,"Invalid Class Room Request!");

            if (quizVM.Name.Length > 85)
                return new ServiceResult(false, string.Empty, "The Quiz Title is too long");

            if (quizVM.StartTime < DateTime.UtcNow)
                return new ServiceResult(false, string.Empty, "The Start Time of Quiz must be in the future or present");

            if (quizVM.Duration > 1440 || quizVM.Duration < 0)
                return new ServiceResult(false, string.Empty, "The Quiz Duration is too high or too small");              


             var message = await _httpClient.PostAsJsonAsync($"Quizes/{quizVM.ClassRoomId}", quizVM);
            if (!message.IsSuccessStatusCode)
            {
                var error = await message.Content.ReadFromJsonAsync<ErrorResponseVM>();
                return new ServiceResult(false,string.Empty,error.Message);
            }
               
            return new ServiceResult(true);
        }
        public async Task<ServiceResult> UpdateQuizAsync(PutQuizVM quizVM, long id)
        {
            if (quizVM.ClassRoomId < 1)
                return new ServiceResult(false, string.Empty, "Invalid Class Room Request!");

            if (quizVM.Name.Length > 85)
                return new ServiceResult(false, "PutQuiz.Name", "The Quiz Title is too long");

            if (quizVM.StartTime.ToUniversalTime() < DateTime.UtcNow)
                return new ServiceResult(false, "PutQuiz.StartTime", "The Start Time of Quiz must be in the future or present");

            if (quizVM.Duration > 1440 || quizVM.Duration < 0)
                return new ServiceResult(false, "PutQuiz.Duration", "The Quiz Duration is Incorrect");

            var message = await _httpClient.PutAsJsonAsync($"Quizes/{quizVM.ClassRoomId}/{id}",quizVM);
            if (!message.IsSuccessStatusCode)
            {
                var error = await message.Content.ReadFromJsonAsync<ErrorResponseVM>();
                return new ServiceResult(false, string.Empty, error.Message);
            }

            return new ServiceResult(true);
        }
    }
}
