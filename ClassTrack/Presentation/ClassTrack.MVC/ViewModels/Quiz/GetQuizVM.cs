

namespace ClassTrack.MVC.ViewModels
{
    public record GetQuizVM(      
        long Id,
        string Name,
        IList<GetChoiceQuestionInQuizVM>  ChoiceQuestions,
        IList<GetOpenQuestionInQuizVM> OpenQuestions,

        DateTime StartTime,
        TimeSpan Duration);   
}
