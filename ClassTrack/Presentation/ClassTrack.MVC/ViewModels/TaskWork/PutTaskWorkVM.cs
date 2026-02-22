



namespace ClassTrack.MVC.ViewModels
{
    public record PutTaskWorkVM(

        string Title,
        string MainPart,
        DateTime EndDate,
        DateTime StartDate,
        TaskWorkAttachmentVM? AttachmentVM = null,
        ICollection<long>? RemovedFileIds = null);
    
}
