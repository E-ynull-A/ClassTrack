using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class TaskWorkService : ITaskWorkService
    {
        private readonly ITaskWorkRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly IClassRoomRepository _roomRepository;

        public TaskWorkService(ITaskWorkRepository taskRepository,
                                IMapper mapper,
                                IClassRoomRepository roomRepository)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
        }

        public async Task<ICollection<GetTaskWorkItemDTO>> GetAllAsync(int page, int take)
        {
            return _mapper.Map<ICollection<GetTaskWorkItemDTO>>(await _taskRepository.GetAll(page: page, take: take).ToListAsync());
        }


        public async Task<ICollection<GetTaskWorkItemDTO>> GetAllByClassRoomIdAsync(int page, int take, long classRoomId)
        {
           return _mapper.Map<ICollection<GetTaskWorkItemDTO>>(await _taskRepository.GetAll(page: page,
                                         take: take,
                                         function: x => x.ClassRoomId == classRoomId)
                                 .ToListAsync());
        }

        public async Task<GetTaskWorkDTO> GetByIdAsync(long id)
        {
            TaskWork taskWork = await _taskRepository.GetByIdAsync(id);

            if(taskWork is null)
            {
                throw new Exception("The Task isn't Found!");
            }

            return _mapper.Map<GetTaskWorkDTO>(taskWork);
        }

        
        public async Task CreateTaskWorkAsync(PostTaskWorkDTO postTask)
        {
            if (!await _roomRepository.AnyAsync(r => r.Id == postTask.ClassRoomId))
            {
                throw new Exception("The ClassRoom isn't Found!");
            }

            _taskRepository.Add(_mapper.Map<TaskWork>(postTask));
            await _taskRepository.SaveChangeAsync();     
        }

        public async Task UpdateTaskWorkAsync(long id,PutTaskWorkDTO putTask)
        {
            if (!await _roomRepository.AnyAsync(r => r.Id == putTask.ClassRoomId))
                throw new Exception("The Class isn't Found!");

            TaskWork edited = await _taskRepository.GetByIdAsync(id);
            if (edited is null)
                throw new Exception("The Task isn't Found!");

            _taskRepository.Update(_mapper.Map(putTask, edited));
            await _taskRepository.SaveChangeAsync();
        }

        public async Task DeleteTaskWorkAsync(long id)
        {
            TaskWork deleted = await _taskRepository.GetByIdAsync(id);
            if (deleted is null)
                throw new Exception("The Task isn't Found!");

            _taskRepository.Delete(deleted);
            await _taskRepository.SaveChangeAsync();
        }
    }
}
