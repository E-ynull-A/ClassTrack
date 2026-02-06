





namespace ClassTrack.MVC.ViewModels
{
   public record PostChoiceQuestionVM(
       
       string Title,
       decimal Point,      
       bool IsMultiple,
       long? QuizId,

       ICollection<PostOptionInChoiceQuestionVM>? Options);
    
}
