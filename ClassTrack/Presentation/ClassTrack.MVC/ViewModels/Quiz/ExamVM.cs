namespace ClassTrack.MVC.ViewModels
{
    public record ExamVM(
        
        GetQuizVM QuizVM,
        PostQuizAnswerVM? QuizAnswer);
   
}
