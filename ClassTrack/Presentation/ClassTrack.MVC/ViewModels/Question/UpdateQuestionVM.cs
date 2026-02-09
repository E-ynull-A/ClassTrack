namespace ClassTrack.MVC.ViewModels
{
    public record UpdateQuestionVM(
        PutChoiceQuestionVM? PutChoice,
        PutOpenQuestionVM? PutOpen);
    
}
