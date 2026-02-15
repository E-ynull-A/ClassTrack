using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IStudentAttendanceClientService
    {
        Task<ServiceResult> CreateAttendanceAsync(IList<PostStudentAttendanceVM> attendanceVM);
        Task<ICollection<GetStudentAttendanceItemVM>?> GetAttendanceAsync(long classRoomId);
    }
}
