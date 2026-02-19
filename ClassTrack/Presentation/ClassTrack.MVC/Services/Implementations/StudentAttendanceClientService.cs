using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Services.Implementations
{
    public class StudentAttendanceClientService:IStudentAttendanceClientService
    {
        private readonly HttpClient _httpClient;

        public StudentAttendanceClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }

        public async Task<ServiceResult> CreateAttendanceAsync(IList<PostStudentAttendanceVM> attendanceVM)
        {
            if (attendanceVM.Any(a => a.ClassRoomId < 1) || attendanceVM.Any(a => a.StudentId < 1))
                return new ServiceResult(false, string.Empty, "Bad Request!");

            if (attendanceVM.DistinctBy(a => a.ClassRoomId).Count() != 1)
                return new ServiceResult(false, string.Empty, "Invalid Class Room Request!");


           HttpResponseMessage message = await _httpClient
                            .PostAsJsonAsync($"StudentAttendances/{attendanceVM.First().ClassRoomId}", attendanceVM);

            if (!message.IsSuccessStatusCode)
            {
                var error = await message.Content.ReadFromJsonAsync<ErrorResponseVM>();
                return new ServiceResult(false, string.Empty, error.Message);
            }

            return new ServiceResult(true);

        }

        public async Task<ICollection<GetStudentAttendanceItemVM>?> GetAttendanceAsync(long classRoomId)
        {
          return await _httpClient.GetFromJsonAsync<ICollection<GetStudentAttendanceItemVM>>($"StudentAttendances/{classRoomId}");
        }
    }
}
