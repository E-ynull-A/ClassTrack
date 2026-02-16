





namespace ClassTrack.MVC.ViewModels
{
    public record GetQuizAnswerItemVM(

        long Id,
        long QuestionId,
        string QuestionTitle,
        long StudentQuizId,
        bool IsEvaluated);
  

}
