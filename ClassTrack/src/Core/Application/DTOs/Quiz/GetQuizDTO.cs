using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetQuizDTO(      
        long Id,
        string Name,
        ICollection<GetChoiceQuestionInQuizDTO>  ChoiceQuestions,
        ICollection<GetOpenQuestionInQuizDTO> OpenQuestions,

        DateTime StartTime,
        TimeSpan Duration);
    
}
