using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClassTrack.MVC.ViewModels
{
    public record UpdateQuizVM(
        PutQuizVM PutQuiz,
        [ValidateNever]
        ICollection<GetQuestionItemVM> PutQuestions);
   
}
