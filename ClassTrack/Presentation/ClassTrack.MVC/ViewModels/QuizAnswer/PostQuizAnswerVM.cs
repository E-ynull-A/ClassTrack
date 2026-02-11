




namespace ClassTrack.MVC.ViewModels
{
    public record PostQuizAnswerVM(
        
        long StudentId,
        long QuizId,
        IList<PostQuizSubmitVM> Answers);
   
}
