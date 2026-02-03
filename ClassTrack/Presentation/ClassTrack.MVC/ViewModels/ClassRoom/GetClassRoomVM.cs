



namespace ClassTrack.MVC.ViewModels
{
    public record GetClassRoomVM(

        long Id,
        string Name,
        string PrivateKey,
        decimal AvgPoint,
        int MemberCount);
    
}
