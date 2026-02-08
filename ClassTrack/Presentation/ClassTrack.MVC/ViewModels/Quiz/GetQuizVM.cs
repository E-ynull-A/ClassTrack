

namespace ClassTrack.MVC.ViewModels
{
    public record GetQuizVM(      
        long Id,
        string Name,
        ICollection<GetChoiceQuestionInQuizVM>  ChoiceQuestions,
        ICollection<GetOpenQuestionInQuizVM> OpenQuestions,

        DateTime StartTime,
        TimeSpan Duration);   
}
