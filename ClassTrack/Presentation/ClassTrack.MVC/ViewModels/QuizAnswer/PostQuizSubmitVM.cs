



namespace ClassTrack.MVC.ViewModels
{
    public record PostQuizSubmitVM(
        
        long QuestionId,
        long? AnswerId,
        IList<long>? AnswerIds,
        string? AnswerText,
        bool IsEvaluated);
   
}
