




namespace ClassTrack.MVC.ViewModels
{
    public record PostTaskWorkVM(

        long ClassRoomId,
        string Title,
        string MainPart,
        DateTime EndDate,
        DateTime StartDate,
        PostTaskWorkAttachmentVM? AttachmentVM);

   
}
