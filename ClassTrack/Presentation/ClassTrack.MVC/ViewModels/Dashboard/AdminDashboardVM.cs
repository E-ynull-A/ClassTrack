namespace ClassTrack.MVC.ViewModels
{
    public record AdminDashboardVM(
        GetStatisticsVM GetStatistics,
        ICollection<GetClassRoomItemVM> GetClassRooms);
    
}
