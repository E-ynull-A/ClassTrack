using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Utilities;
using ClassTrack.Persistance.Implementations.Repositories;
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
        private readonly IStudentRepository _studentRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IStudentService _studentService;

        public TaskWorkService(ITaskWorkRepository taskRepository,
                                IMapper mapper,
                                IClassRoomRepository roomRepository,
                                IFileService cloudService,
                                ITaskWorkAttachmentRepository attachmentRepository,
                                IStudentRepository studentRepository,
                                ICurrentUserService currentUser,
                                IStudentService studentService)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
            _cloudService = cloudService;
            _attachmentRepository = attachmentRepository;
            _studentRepository = studentRepository;
            _currentUser = currentUser;
            _studentService = studentService;
        }

        public async Task<GetTaskWorkItemPagedDTO> GetAllAsync(int page, int take)
        {
            return new GetTaskWorkItemPagedDTO(_mapper.Map<ICollection<GetTaskWorkItemDTO>>(await _taskRepository
                                            .GetAll(page: page, take: take).ToListAsync()),await _taskRepository.CountAsync());
        }

        public async Task<GetTaskWorkItemPagedDTO> GetAllByClassRoomIdAsync(int page, int take, long classRoomId)
        {
            return new GetTaskWorkItemPagedDTO(_mapper.Map<ICollection<GetTaskWorkItemDTO>>(await _taskRepository.GetAll(
                                                  page: page,
                                                  take: take,
                                                  function: x => x.ClassRoomId == classRoomId)
                                                    .ToListAsync()), await _taskRepository.CountAsync(t=>t.ClassRoomId == classRoomId));
        }

        public async Task<GetStudentTaskWorkDTO> GetStudentTaskWorkAsync(long taskWorkId,long studentId)
        {

            StudentTaskWork? studentTask = await _taskRepository.GetStudentTaskWorkAsync(taskWorkId,studentId);

            if (studentTask is null)
                throw new NotFoundException("The Task Work not Found!");

            return _mapper.Map<GetStudentTaskWorkDTO>(studentTask);

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

        public async Task<ICollection<GetTaskWorkAttachmentDTO>> GetAttachmentAsync(long taskWorkId)
        {
            return _mapper.Map<ICollection<GetTaskWorkAttachmentDTO>>(await _attachmentRepository
                                                                  .GetAll(function: a => a.TaskWorkId == taskWorkId)
                                                                  .ToListAsync());
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
                await _postTaskWorkAttachmentAsync(newTask, postTask.AttachmentDTO.Files);
            }

            ICollection<Student> students = await _studentRepository
                                                    .GetAll(function: s => s.StudentClasses
                                                            .Select(sc => sc.ClassRoomId).
                                                            Contains(postTask.ClassRoomId)).ToListAsync();

            newTask.StudentTaskWorks = students.Select(s => new StudentTaskWork
            {
                StudentId = s.Id
            }).ToImmutableList();


            _taskRepository.Add(newTask);
            await _taskRepository.SaveChangeAsync();
        }

        public async Task UpdateTaskWorkAsync(long id, long classRoomId, PutTaskWorkDTO putTask)
        {
            if (!await _roomRepository.AnyAsync(r => r.Id == classRoomId))
                throw new NotFoundException("The Class isn't Found!");

            TaskWork edited = await _taskRepository.GetByIdAsync(id, includes: ["TaskWorkAttachments"]);
            if (edited is null)
                throw new NotFoundException("The Task isn't Found!");

            if (putTask.AttachmentDTO is not null)
                await _postTaskWorkAttachmentAsync(edited, putTask.AttachmentDTO.Files);

            if (putTask.RemovedFileIds is not null)
            {
                ICollection<TaskWorkAttachment> attachs = await _attachmentRepository
                                                           .GetAll(function:
                                                                            a => putTask.RemovedFileIds                                                                   
                                                                                      .Contains(a.Id))
                                                           .ToListAsync();

                if (attachs.Count == putTask.RemovedFileIds.Count)
                    _attachmentRepository.DeleteRange(attachs);
                else
                    throw new NotFoundException("The File Not Found");
            }

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

        public async Task StudentSubmitAsync(PutStudentTaskWorkDTO studentSubmit,long taskWorkId)
        {
            string userId = _currentUser.GetUserId();

            TaskWork? submittedTask = await _taskRepository.FirstOrDefaultAsync(t=>t.Id == taskWorkId 
                                                                                       && 
                                                                                   t.StudentTaskWorks.Select(st=>st.Student.AppUserId).Contains(userId),
                                                                                   includes: ["StudentTaskWorks"]);

            if (submittedTask is null)
                throw new NotFoundException("The Task Not Found");

           if(submittedTask.EndDate.ToUniversalTime() < DateTime.UtcNow)            
                throw new BadRequestException("The DeadLine is over!");
            if (submittedTask.StartDate.ToUniversalTime() > DateTime.UtcNow)
                throw new BadRequestException("The Quiz does not start!");

            string? text = string.IsNullOrWhiteSpace(studentSubmit.StudentAnswerText) ? null : studentSubmit.StudentAnswerText.Trim();
            string? link = string.IsNullOrWhiteSpace(studentSubmit.StudentAnswerLink) ? null : studentSubmit.StudentAnswerLink.Trim();
            StudentTaskWork taskWork = submittedTask.StudentTaskWorks.First();

            taskWork.StudentAnswerLink = string.IsNullOrWhiteSpace(link) ? null : link.StartsWith("http") ? link : string.Concat("https://", link);
            taskWork.StudentAnswerText = text;

            if (text is null && link is null)
            {
                taskWork.IsEvaluated = true;
                taskWork.Point = 0;
            }

            _taskRepository.Update(submittedTask);
            await _taskRepository.SaveChangeAsync();
        }

        public async Task EvaulateTaskAsync(PutPointInTaskWorkDTO putPoint,long taskWorkId,long studentId)
        {

            if (putPoint.Point < 0 || putPoint.Point > 100)
                throw new BadRequestException("Invalid Point Request");

            TaskWork? evaultedTask = await _taskRepository.FirstOrDefaultAsync(t => t.Id == taskWorkId
                                                                           &&
                                                                       t.StudentTaskWorks.Select(st => st.StudentId).Contains(studentId),
                                                                       includes: ["StudentTaskWorks"]);


            if (evaultedTask.EndDate.ToUniversalTime() > DateTime.UtcNow)
                throw new BadRequestException("The Quiz does not over!");

            StudentTaskWork taskWork = evaultedTask.StudentTaskWorks.First();

            taskWork.Point = putPoint.Point;
            taskWork.IsEvaluated = true;

            _taskRepository.Update(evaultedTask);
            await _taskRepository.SaveChangeAsync();

            await _studentService.CalculateAvgPoint(studentId, evaultedTask.ClassRoomId);
        }

        public async Task SoftQuizDeleteAsync(long id)
        {
            TaskWork? deleted = await _taskRepository.GetByIdAsync(id);

            if (deleted == null)
                throw new NotFoundException("The TaskWork isn't Found!");

            deleted.IsDeleted = true;
            _taskRepository.Update(deleted);
            await _taskRepository.SaveChangeAsync();
        }

        private async Task _postTaskWorkAttachmentAsync(TaskWork taskWork, ICollection<IFormFile> files)
        {
            foreach (IFormFile file in files)
            {
                CloudinaryResponceDTO responceDTO = await _cloudService.UploadAsync(file);

                TaskWorkAttachment attachment = _mapper.Map<TaskWorkAttachment>(responceDTO);

                _attachmentRepository.Add(attachment);
                taskWork.TaskWorkAttachments.Add(attachment);
            }
        }
    }
}
