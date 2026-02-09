

namespace ClassTrack.MVC.ViewModels
{
    public record GetOptionForUpdateVM(
        long Id,
        string Variant,
        bool IsCorrect,
        bool IsDeleted);
    
}
