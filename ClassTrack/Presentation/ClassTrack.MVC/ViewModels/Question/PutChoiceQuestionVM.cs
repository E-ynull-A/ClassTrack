

namespace ClassTrack.MVC.ViewModels
{
    public record PutChoiceQuestionVM(
       string Title,
       decimal Point,
       bool IsMultiple,
       ICollection<PutOptionInChoiceQuestionVM> Options);

}
