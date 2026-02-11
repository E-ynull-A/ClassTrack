

namespace ClassTrack.MVC.ViewModels
{
    public record GetChoiceQuestionInQuizVM(

        long Id,
        string Title,
        decimal Point,
        bool IsMultiple,
        string QuestionType,
        IList<GetOptionItemInChoiceQuestionVM>? Options = null);
   
}
