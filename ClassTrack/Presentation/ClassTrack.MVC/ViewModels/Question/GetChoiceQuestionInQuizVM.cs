

namespace ClassTrack.MVC.ViewModels
{
    public record GetChoiceQuestionInQuizVM(

        long Id,
        string Title,
        decimal Point,
        string QuestionType,
        ICollection<GetOptionItemInChoiceQuestionVM>? Options = null);
   
}
