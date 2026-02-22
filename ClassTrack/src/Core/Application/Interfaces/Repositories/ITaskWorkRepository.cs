using ClassTrack.Domain.Entities;


namespace ClassTrack.Application.Interfaces.Repositories
{
    public interface ITaskWorkRepository:IRepository<TaskWork>
    {
        Task<StudentTaskWork?> GetStudentTaskWorkAsync(long taskWorkId, string userId);
    }
}
