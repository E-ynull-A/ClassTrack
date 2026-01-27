using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class TaskWorkService:ITaskWorkService
    {
        private readonly ITaskWorkRepository _taskRepository;

        public TaskWorkService(ITaskWorkRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        //public ICollection<GetTaskWorkItemDTO> GetAllAsync(int page,int take)
        //{

        //}


    }
}
