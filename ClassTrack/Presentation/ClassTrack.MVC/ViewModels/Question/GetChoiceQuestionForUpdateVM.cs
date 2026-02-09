

namespace ClassTrack.MVC.ViewModels
{
    public record GetChoiceQuestionForUpdateVM(
        long Id,
        string Title,
        decimal Point,
        bool IsMultiple,
        ICollection<GetOptionForUpdateVM> Options);


}
