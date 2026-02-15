





namespace ClassTrack.MVC.ViewModels
{
    public record GetQuizAnswerItemVM(

        long QuestionId,
        string QuestionTitle,
        long StudentQuizId,
        bool IsEvaluated);
  

}
