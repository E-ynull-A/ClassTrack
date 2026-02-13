using ClassTrack.MVC.Services.Interfaces;

namespace ClassTrack.MVC.Services.Implementations
{
    public class StudentAttendanceClientService:IStudentAttendanceClientService
    {
        private readonly HttpClient _httpClient;

        public StudentAttendanceClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }


    }
}
