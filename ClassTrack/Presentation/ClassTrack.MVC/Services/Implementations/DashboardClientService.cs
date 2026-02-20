using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Services.Implementations
{
    public class DashboardClientService:IDashboardClientService
    {
        private readonly HttpClient _httpClient;
        public DashboardClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }

        public async Task<AdminDashboardVM> GetDasboardAsync()
        {
            return new AdminDashboardVM(await _httpClient.GetFromJsonAsync<GetStatisticsVM>("Statistics"),
                                        await _httpClient.GetFromJsonAsync<ICollection<GetClassRoomItemVM>>("ClassRooms"));
        }

        public async Task<GetUserPagedItemVM> GetUserAllAsync(int page)
        {
           return await _httpClient.GetFromJsonAsync<GetUserPagedItemVM>($"Statistics/Users?page={page}");
        }

        public async Task<ServiceResult> BanUserAsync(PostBanUserVM postBan)
        {            
            if (postBan.Duration < 0)
                return new ServiceResult(false, string.Empty, "Duration cannot be negative");

            if(!(postBan.Unit == "Hour" || postBan.Unit == "Day"))
                return new ServiceResult(false, string.Empty, "Bad Unit Request!");


            HttpResponseMessage message = await _httpClient.PatchAsJsonAsync("Statistics",postBan);
            if (!message.IsSuccessStatusCode)
            {
                throw new Exception(await message.Content.ReadAsStringAsync());
            }

            return new ServiceResult(true);
        }
    }
}
