namespace ClassTrack.MVC.ViewModels
{
    public record ServiceResult(
         bool Ok,
         string? ErrorKey=null,
         string? ErrorMessage=null);
   
}
