

namespace ClassTrack.MVC.ViewModels
{
    public record PutOptionInChoiceQuestionVM(
        long? Id,
        string Variant,        
        bool IsDeleted,
        bool IsCorrect);
    
}
