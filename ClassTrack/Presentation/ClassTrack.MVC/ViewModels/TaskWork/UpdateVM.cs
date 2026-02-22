namespace ClassTrack.MVC.ViewModels
{
    public record UpdateVM(
        
    ICollection<GetTaskWorkAttachmentVM> WorkAttachmentVMs,
    PutTaskWorkVM PutTaskWorkVM);
 
}
