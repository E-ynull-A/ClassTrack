using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetChoiceQuestionInQuizDTO(
        long Id,
        string QuestionType,
        string Title,
        decimal Point,
        ICollection<GetOptionItemInQuizDTO>? Options = null
        );
    
}
