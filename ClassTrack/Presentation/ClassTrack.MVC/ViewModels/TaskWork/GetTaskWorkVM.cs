



namespace ClassTrack.MVC.ViewModels
{
    public record GetTaskWorkVM(

        long Id,
        long ClassRoomId,
        string Title,
        string MainPart,
        DateTime EndDate,
        DateTime StartDate,
        ICollection<GetTaskWorkAttachmentVM> TaskWorkAttachments);
   
}
