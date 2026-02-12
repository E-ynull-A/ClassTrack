using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClassTrack.MVC.ViewModels
{
    public record ExamVM(

        [ValidateNever]
        GetQuizVM? QuizVM,
        PostQuizAnswerVM QuizAnswer = null);
   
}
