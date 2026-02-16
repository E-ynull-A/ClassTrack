


namespace ClassTrack.MVC.ViewModels
{
    public record GetQuestionInQuizAnswerVM(

        long Id,
        string QuestionTitle,
        decimal Point,
        ICollection<GetOptionItemInQuestionVM>? Options = null);
 
}
