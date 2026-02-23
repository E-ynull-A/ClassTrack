namespace ClassTrack.Application.DTOs
{
    public record GetOpenQuestionInQuizDTO(
        
        long Id,
        string Title,
        decimal Point,
        string QuestionType);
   
}
