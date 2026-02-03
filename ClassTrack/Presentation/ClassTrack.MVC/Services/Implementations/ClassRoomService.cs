using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Security.Claims;


namespace ClassTrack.MVC.Services.Implementations
{
    public class ClassRoomService:IClassRoomService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _accessor;

        public ClassRoomService(IHttpClientFactory httpClient,
                                IHttpContextAccessor accessor)
        {
            _httpClient = httpClient.CreateClient("ClassTrackClient");
            _accessor = accessor;
        }

        public async Task<IEnumerable<GetClassRoomItemVM>> GetAllAsync(int page = 1, int take = 5)
        {
            IEnumerable<GetClassRoomItemVM>? response = await _httpClient
                                    .GetFromJsonAsync<IEnumerable<GetClassRoomItemVM>>
                                                ($"ClassRooms?page={page}&take={take}");

            return response;
        }

        public async Task<GetClassRoomWithPermissionVM> GetByIdAsync(long id)
        {
            string? userId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                throw new Exception("User isn't Found!");

            GetClassRoomWithPermissionVM roomWithPermissionVM = new GetClassRoomWithPermissionVM
            (   await _httpClient.GetFromJsonAsync<IsTeacherVM>($"Permissions?classRoomId={id}"),
                await _httpClient.GetFromJsonAsync<GetClassRoomVM>($"ClassRooms/{id}"),
                await _httpClient.GetFromJsonAsync<GetTeacherClassRoomItemVM>($"Teacher/{userId}"));
                
            return roomWithPermissionVM;
        }
    }
}
