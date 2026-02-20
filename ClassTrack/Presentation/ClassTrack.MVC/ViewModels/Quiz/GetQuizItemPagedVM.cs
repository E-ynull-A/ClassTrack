



namespace ClassTrack.MVC.ViewModels;

public record GetQuizItemPagedVM(
    ICollection<GetQuizItemVM> QuizItems,
    int TotalCount);

