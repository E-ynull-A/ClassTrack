




namespace ClassTrack.MVC.ViewModels
{
    public record PostQuizAnswerVM(
        
        long QuizId,
        IList<PostQuizSubmitVM> Answers);
   
}
