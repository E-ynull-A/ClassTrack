using ClassTrack.Application.DTOs;




namespace ClassTrack.Application.Interfaces.Services
{
    public interface IStudentAttendanceService
    {
        Task CreateAttendanceAsync(ICollection<PostStudentAttendanceDTO> attendanceDTOs);
        Task<ICollection<GetStudentAttendanceItemDTO>> GetAllAsync(int page, int take, long classRoomId);
        Task UpdateAttendanceAsync(ICollection<PutStudentAttendanceDTO> attendanceDTOs);
        Task DeleteAttendanceAsync(long id);
    }
}
