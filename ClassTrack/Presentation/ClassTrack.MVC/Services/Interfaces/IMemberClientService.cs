using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IMemberClientService
    {
        Task<GetMemberItemVM> GetMembersAsync(long classRoomId);
        Task<ICollection<GetStudentItemVM>?> GetStudentListAsync(long? quizId, long? taskWorkId, long classRoomId);
        Task<ICollection<GetSimpleStudentItemVM>?> GetSimpleStudentAsync(long classRoomId);
        Task<ServiceResult> KickAsync(long classRoomId, long studentId);
        Task<ServiceResult> PromoteAsync(long classRoomId, long studentId);
    }
}
