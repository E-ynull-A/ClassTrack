namespace ClassTrack.MVC.ViewModels
{
    public record GetClassRoomItemVM(

        long Id,
        string Name,
        decimal AvgPoint,
        int StudentCount,
        ICollection<string> TeacherFullNames,
        DateTime CreatedAt);

}
