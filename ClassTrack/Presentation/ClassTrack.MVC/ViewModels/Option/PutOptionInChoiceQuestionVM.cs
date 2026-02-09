

namespace ClassTrack.MVC.ViewModels
{
    public record PutOptionInChoiceQuestionVM(
        long? Id,
        string Variant,
        bool IsCorrect,
        bool IsDeleted);
    
}
