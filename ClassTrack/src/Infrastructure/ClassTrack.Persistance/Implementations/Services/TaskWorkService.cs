using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class TaskWorkService : ITaskWorkService
    {
        private readonly ITaskWorkRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly IClassRoomRepository _roomRepository;
        private readonly IFileService _cloudService;
        private readonly ITaskWorkAttachmentRepository _attachmentRepository;

        public TaskWorkService(ITaskWorkRepository taskRepository,
                                IMapper mapper,
                                IClassRoomRepository roomRepository,
                                IFileService cloudService,
                                ITaskWorkAttachmentRepository attachmentRepository)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
            _cloudService = cloudService;
            _attachmentRepository = attachmentRepository;
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
            TaskWork taskWork = await _taskRepository.GetByIdAsync(id, includes: [nameof(TaskWork.TaskWorkAttachments)]);

            if (taskWork is null)
            {
                throw new NotFoundException("The Task isn't Found!");
            }

            return _mapper.Map<GetTaskWorkDTO>(taskWork);
        }


        public async Task CreateTaskWorkAsync(PostTaskWorkDTO postTask)
        {
            if (!await _roomRepository.AnyAsync(r => r.Id == postTask.ClassRoomId))
            {
                throw new NotFoundException("The ClassRoom isn't Found!");
            }

            TaskWork newTask = _mapper.Map<TaskWork>(postTask);
            newTask.TaskWorkAttachments = new Collection<TaskWorkAttachment>();

            if (postTask.AttachmentDTO is not null)
            {                
                foreach (IFormFile file in postTask.AttachmentDTO.Files)
                {              
                    CloudinaryResponceDTO responceDTO = await _cloudService.UploadAsync(file);

                    TaskWorkAttachment attachment = _mapper.Map<TaskWorkAttachment>(responceDTO);
                    
                    _attachmentRepository.Add(attachment);
                    newTask.TaskWorkAttachments.Add(attachment);
                }
            }

            _taskRepository.Add(newTask);
            await _taskRepository.SaveChangeAsync();
        }

        public async Task UpdateTaskWorkAsync(long id, PutTaskWorkDTO putTask)
        {
            if (!await _roomRepository.AnyAsync(r => r.Id == putTask.ClassRoomId))
                throw new NotFoundException("The Class isn't Found!");

            TaskWork edited = await _taskRepository.GetByIdAsync(id);
            if (edited is null)
                throw new NotFoundException("The Task isn't Found!");

            _taskRepository.Update(_mapper.Map(putTask, edited));
            await _taskRepository.SaveChangeAsync();
        }

        public async Task DeleteTaskWorkAsync(long id)
        {
            TaskWork deleted = await _taskRepository.GetByIdAsync(id);
            if (deleted is null)
                throw new NotFoundException("The Task isn't Found!");

            _taskRepository.Delete(deleted);
            await _taskRepository.SaveChangeAsync();
        }
    }
}
