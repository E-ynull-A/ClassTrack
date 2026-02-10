using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Services.Implementations
{
    public class MemberClientService:IMemberClientService
    {
        private readonly HttpClient _httpClient;
        public MemberClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }

        public async Task<GetMemberItemVM> GetMembersAsync(long classRoomId)
        {
            return new GetMemberItemVM(await _httpClient.GetFromJsonAsync
                                                        <ICollection<GetStudentItemVM>>
                                                                    ($"Students/{classRoomId}"),
                                       await _httpClient.GetFromJsonAsync
                                                        <ICollection<GetTeacherItemVM>>
                                                                    ($"Teachers/{classRoomId}"));
                  
        }

        public async Task<ServiceResult> KickAsync(long classRoomId,long studentId)
        {
           HttpResponseMessage message = await _httpClient.DeleteAsync($"Students/{classRoomId}/{studentId}");

            if (!message.IsSuccessStatusCode)
                return new ServiceResult(false, string.Empty, await message.Content.ReadAsStringAsync());

            return new ServiceResult(true);
        }
    }
}
