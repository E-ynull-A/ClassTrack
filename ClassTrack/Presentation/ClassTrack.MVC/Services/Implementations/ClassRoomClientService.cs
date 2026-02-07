using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ClassTrack.MVC.Services.Implementations
{
    public class ClassRoomService : IClassRoomClientService
    {
        private readonly HttpClient _httpClient;

        public ClassRoomService(IHttpClientFactory httpClient)

        {
            _httpClient = httpClient.CreateClient("ClassTrackClient");
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
               (await _httpClient.GetFromJsonAsync<IsTeacherVM>($"Permissions?classRoomId={id}"),
                await _httpClient.GetFromJsonAsync<GetClassRoomVM>($"ClassRooms/{id}"));

            return roomWithPermissionVM;
        }

        public async Task<ServiceResult> CreateClassRoomAsync(PostClassRoomVM classRoomVM)
        {

            if (classRoomVM.Name.Length > 150)
            {
                return new ServiceResult(false, nameof(PostClassRoomVM.Name),
                                        "The Name Length is too Long!");
            }

            var message = await _httpClient.PostAsJsonAsync("ClassRooms", classRoomVM);

            if (!message.IsSuccessStatusCode)
            {
                var errorContent = await message.Content.ReadAsStringAsync();
                return new ServiceResult(false, string.Empty, errorContent);
            }

            return new ServiceResult(true);
        }

        public async Task<ServiceResult> JoinClassRoomAsync(JoinClassRoomVM joinClass)
        {

            if (joinClass.ClassKey.Length != 8)
                return new ServiceResult(false, nameof(JoinClassRoomVM.ClassKey), "There is no any Class Room in this Key");
            if (!joinClass.ClassKey.All(Char.IsLetterOrDigit))
                return new ServiceResult(false, nameof(JoinClassRoomVM.ClassKey), "There is no any Class Room in this Key");

             var message = await _httpClient.PostAsJsonAsync("Students", joinClass);

            if (!message.IsSuccessStatusCode)
            {
                var errorContent = await message.Content.ReadAsStringAsync();
                return new ServiceResult(false, nameof(JoinClassRoomVM.ClassKey), errorContent);
            }

            return new ServiceResult(true);
        }
        public async Task DeleteClassRoomAsync(long id)
        {
            await _httpClient.DeleteAsync($"ClassRooms?id={id}");
        }
    }
}
