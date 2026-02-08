






namespace ClassTrack.MVC.ViewModels
{
    public record PostQuizVM(

       string Name,
       DateTime StartTime,
       double Duration,
       long ClassRoomId,

       ICollection<PostChoiceQuestionVM>? ChoiceQuestions,
       ICollection<PostOpenQuestionVM>? OpenQuestions);
    
}
