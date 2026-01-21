



using ClassTrack.Application.Validators;
using Microsoft.AspNetCore.Mvc;

namespace ClassTrack.Application.DTOs
{
   public record PostChoiceQuestionDTO(
       
       string Title,
       decimal Point,      
       bool IsMultiple,
       long QuizId,

       ICollection<PostOptionInChoiceQuestionDTO>? Options = null
       )
    {
        public PostChoiceQuestionDTO() : this(default!, default, default, default, null) { }
    };
   
}
