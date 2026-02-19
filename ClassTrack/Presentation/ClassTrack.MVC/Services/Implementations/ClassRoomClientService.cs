using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
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
        public async Task<GetClassRoomWithPermissionVM> GetByIdAsync(long classRoomId)
        {
            GetClassRoomWithPermissionVM roomWithPermissionVM = new GetClassRoomWithPermissionVM
               (await _httpClient.GetFromJsonAsync<IsTeacherVM>($"Permissions/{classRoomId}"),
                await _httpClient.GetFromJsonAsync<GetClassRoomVM>($"ClassRooms/{classRoomId}"),
                null);
          
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
                var errorContent = await message.Content.ReadFromJsonAsync<ErrorResponseVM>();
                return new ServiceResult(false, "PostClass.Name" ,errorContent.Message);
            }

            return new ServiceResult(true);
        }
        public async Task<ServiceResult> JoinClassRoomAsync(JoinClassRoomVM joinClass)
        {

            if (joinClass.ClassKey.Trim().Length != 8)
                return new ServiceResult(false, "JoinClass.ClassKey", "There is no any Class Room in this Key");
            if (!joinClass.ClassKey.All(Char.IsLetterOrDigit))
                return new ServiceResult(false, "JoinClass.ClassKey", "There is no any Class Room in this Key");

             var message = await _httpClient.PostAsJsonAsync("Students", joinClass);

            if (!message.IsSuccessStatusCode)
            {
                var error = await message.Content.ReadFromJsonAsync<ErrorResponseVM>();
                return new ServiceResult(false, "JoinClass.ClassKey", error.Message);
            }

            return new ServiceResult(true);
        }
        public async Task DeleteClassRoomAsync(long classRoomId)
        {
            await _httpClient.DeleteAsync($"ClassRooms/{classRoomId}");
        }
        public async Task<ServiceResult> UpdateClassRoomAsync(long classRoomId,PutClassRoomVM roomVM)
        {
            HttpResponseMessage message = await _httpClient.PutAsJsonAsync($"ClassRooms/{classRoomId}", roomVM);

            if (!message.IsSuccessStatusCode)
            {
                var result = await message.Content.ReadFromJsonAsync<ErrorResponseVM>();

                return new ServiceResult(false, 
                                "PutClassRoom.Name",
                                result.Message);
            }

            return new ServiceResult(true);
        }
    }
}
