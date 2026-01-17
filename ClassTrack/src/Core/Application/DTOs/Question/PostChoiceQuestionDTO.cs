



namespace ClassTrack.Application.DTOs
{
   public record PostChoiceQuestionDTO(
       
       string Title,
       decimal Point,
       ICollection<PostOptionInChoiceQuestionDTO> Options,
       bool IsMultiple,

       long QuizId);
   
}
