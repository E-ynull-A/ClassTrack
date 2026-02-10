using ClassTrack.MVC.ViewModels;

namespace ClassTrack.MVC.Services.Interfaces
{
    public interface IMemberClientService
    {
        Task<GetMemberItemVM> GetMembersAsync(long classRoomId);
        Task<ServiceResult> KickAsync(long classRoomId, long studentId);
    }
}
