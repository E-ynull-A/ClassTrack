using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClassTrack.MVC.ViewModels
{
    public record EvaulateVM(

        [ValidateNever]
        GetQuizAnswerVM GetQuiz,
        PutQuizAnswerVM PutQuizVM = null);
    
}
