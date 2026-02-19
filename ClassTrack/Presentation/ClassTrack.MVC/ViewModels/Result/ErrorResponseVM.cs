namespace ClassTrack.MVC.ViewModels
{
    public record ErrorResponseVM(
        
        int StatusCode,
        string Message,
        string Detail,

        IDictionary<string, string[]>? Errors = null);
   
}
