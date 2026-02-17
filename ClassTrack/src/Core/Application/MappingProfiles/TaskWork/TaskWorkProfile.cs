using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Domain.Entities;


namespace ClassTrack.Application.MappingProfiles
{
    public class TaskWorkProfile:Profile
    {
        public TaskWorkProfile()
        {
            CreateMap<TaskWork, GetTaskWorkItemDTO>();
            CreateMap<TaskWork, GetTaskWorkDTO>();

            CreateMap<PostTaskWorkDTO, TaskWork>()
                .ForMember(nameof(TaskWork.TaskWorkAttachments),opt=>opt.Ignore());

            CreateMap<PutTaskWorkDTO,TaskWork>();

            CreateMap<TaskWorkAttachment, GetTaskWorkAttachmentDTO>();
        }
    }
}
