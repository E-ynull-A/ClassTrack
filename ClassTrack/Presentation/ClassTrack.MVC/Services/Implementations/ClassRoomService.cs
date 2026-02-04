using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ClassTrack.MVC.Services.Implementations
{
    public class ClassRoomService:IClassRoomService
    {
        private readonly HttpClient _httpClient;

        public ClassRoomService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("ClassTrackClient");
        }

        public async Task LogoutAsync()
        {
           await _httpClient.DeleteAsync("Accounts/Logout");
        }

        public async Task<IEnumerable<GetClassRoomItemVM>> GetAllAsync()
        {
            IEnumerable<GetClassRoomItemVM>? response = await _httpClient
                    .GetFromJsonAsync<IEnumerable<GetClassRoomItemVM>>("ClassRooms");

            return response;
        }

        public async Task<GetClassRoomWithPermissionVM> GetByIdAsync(long id)
        {

            GetClassRoomWithPermissionVM roomWithPermissionVM = new GetClassRoomWithPermissionVM
            (   await _httpClient.GetFromJsonAsync<IsTeacherVM>($"Permissions?classRoomId={id}"),
                await _httpClient.GetFromJsonAsync<GetClassRoomVM>($"ClassRooms/{id}"),
                await _httpClient.GetFromJsonAsync<IEnumerable<GetClassRoomItemVM>>($"ClassRooms"));
                
            return roomWithPermissionVM;
        }

        public async Task CreateClassRoomAsync(PostClassRoomVM classRoomVM)
        {
            var message = await _httpClient.PostAsJsonAsync("ClassRooms", classRoomVM);
            if (!message.IsSuccessStatusCode)
            {
                var errorContent = await message.Content.ReadAsStringAsync();
                Console.WriteLine(errorContent);
            }
        }
    }
}
