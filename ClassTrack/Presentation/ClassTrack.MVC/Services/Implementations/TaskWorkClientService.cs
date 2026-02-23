using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Services.Implementations
{
    public class TaskWorkClientService : ITaskWorkClientService
    {
        private readonly HttpClient _httpClient;
        public TaskWorkClientService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ClassTrackClient");
        }
        public async Task<GetTaskWorkItemWithPermissionVM?> GetAllAsync(int page,int take,long classRoomId)
        {
           return new GetTaskWorkItemWithPermissionVM( 
                        await _httpClient.GetFromJsonAsync<GetTaskWorkItemPagedVM>($"TaskWorks/{classRoomId}/ClassRoom?page={page}&take={take}"),
                        await _httpClient.GetFromJsonAsync<IsTeacherVM>($"Permissions/{classRoomId}"));
        }
        public async Task<GetTaskWorkVM?> GetByIdAsync(long classRoomId,long id)
        {
           return await _httpClient.GetFromJsonAsync<GetTaskWorkVM>($"TaskWorks/{classRoomId}/{id}/Detail");
        }
        public async Task<ServiceResult> CreateAsync(long classRoomId, PostTaskWorkVM taskWorkVM)
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(taskWorkVM.ClassRoomId.ToString()), "ClassRoomId");
                content.Add(new StringContent(taskWorkVM.Title), "Title");
                content.Add(new StringContent(taskWorkVM.MainPart), "MainPart");
                content.Add(new StringContent(taskWorkVM.EndDate.ToString()), "EndDate");
                content.Add(new StringContent(taskWorkVM.StartDate.ToString()), "StartDate");
               

                if (taskWorkVM.AttachmentVM is not null)
                {
                    ImmutableList<IFormFile> files = taskWorkVM.AttachmentVM.Files.ToImmutableList();
                    for (int i = 0;i < taskWorkVM.AttachmentVM.Files.Count;i++)
                    {
                        StreamContent fileContent = new StreamContent(files[i].OpenReadStream());

                        content.Add(fileContent, $"AttachmentDTO.Files", files[i].FileName);
                    }
                }

                HttpResponseMessage message = await _httpClient.PostAsync($"TaskWorks/{classRoomId}", content);

                if (!message.IsSuccessStatusCode)
                {
                    var error = await message.Content.ReadFromJsonAsync<ErrorResponseVM>();
                    return new ServiceResult(false, string.Empty, error.Message);
                }

                return new ServiceResult(true);
            };    
        }
        public async Task<ServiceResult> UpdateAsync(long id,long classRoomId,PutTaskWorkVM putTaskWork)
        {
            if(string.IsNullOrEmpty(putTaskWork.Title.Trim()) 
                || putTaskWork.Title.Length > 200)
            {
                return new ServiceResult(false,"Title","The Length of Title is Invalid!");
            }

            if (putTaskWork.StartDate.ToUniversalTime() <= DateTime.UtcNow)
                return new ServiceResult(false, "StartDate", "Task doesn't begin in past");
            if (putTaskWork.StartDate >= putTaskWork.EndDate)
                return new ServiceResult(false, string.Empty, "The Task must ended after the Start Time");

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(putTaskWork.Title), "Title");
                content.Add(new StringContent(putTaskWork.MainPart), "MainPart");
                content.Add(new StringContent(putTaskWork.EndDate.ToString()), "EndDate");
                content.Add(new StringContent(putTaskWork.StartDate.ToString()), "StartDate");

                if(putTaskWork.RemovedFileIds is not null)
                    for(int i = 0;i< putTaskWork.RemovedFileIds.Count; i++)
                    {
                        content.Add(new StringContent(putTaskWork.RemovedFileIds.ToImmutableList()[i].ToString()), "RemovedFileIds");
                    }
                    


                if (putTaskWork.AttachmentVM is not null)
                {
                    ImmutableList<IFormFile> files = putTaskWork.AttachmentVM.Files.ToImmutableList();
                    for (int i = 0; i < putTaskWork.AttachmentVM.Files.Count; i++)
                    {
                        StreamContent fileContent = new StreamContent(files[i].OpenReadStream());
                        content.Add(fileContent, $"AttachmentDTO.Files", files[i].FileName);
                    }
                }
                HttpResponseMessage message = await _httpClient.PutAsync($"TaskWorks/{classRoomId}/{id}", content);

                if (!message.IsSuccessStatusCode)
                    return new ServiceResult(false, string.Empty, (await message.Content.ReadFromJsonAsync<ErrorResponseVM>())?.Message);

                return new ServiceResult(true);
            }  
        }
        public async Task<ICollection<GetTaskWorkAttachmentVM>?> GetAllTaskAttachmentAsync(long classRoomId,long taskWorkId)
        {
            return await _httpClient.GetFromJsonAsync<ICollection<GetTaskWorkAttachmentVM>>($"TaskWorkAttachments/{classRoomId}/{taskWorkId}");
        }
        public async Task StudentSubmitAsync(long classRoomId,long taskWorkId,PutStudentTaskWorkVM studentTask)
        {        
            HttpResponseMessage message = await _httpClient
                        .PutAsJsonAsync($"TaskWorks/{classRoomId}/{taskWorkId}/Submit",studentTask);

            if (!message.IsSuccessStatusCode)
                throw new Exception((await message.Content.ReadFromJsonAsync<ErrorResponseVM>())?.Message);             
        }
        public async Task<GetStudentTaskWorkVM?> GetStudentAnswerAsync(long taskWorkId,long classRoomId,long studentId)
        {
          return await _httpClient
                    .GetFromJsonAsync
                    <GetStudentTaskWorkVM>($"StudentTaskWorks/{classRoomId}/{taskWorkId}/{studentId}");
        }
        public async Task EvaulateAsync(long classRoomId,long taskWorkId,long studentId,PutPointInTaskWorkVM putPointVM)
        {
            if (putPointVM.Point < 0 || putPointVM.Point > 100)
                throw new Exception("Invalid Point Input");

           var message = await _httpClient
                .PutAsJsonAsync($"StudentTaskWorks/{classRoomId}/{taskWorkId}/{studentId}",putPointVM);

            if (!message.IsSuccessStatusCode)
                throw new Exception((await message.Content.ReadFromJsonAsync<ErrorResponseVM>())?.Message);
        }
        public async Task SoftDeleteAsync(long id, long classRoomId)
        {
           await _httpClient.DeleteAsync($"TaskWorks/{classRoomId}/{id}/Restore");
        }
    }
}
