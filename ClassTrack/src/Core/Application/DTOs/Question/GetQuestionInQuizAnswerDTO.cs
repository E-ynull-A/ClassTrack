using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetQuestionInQuizAnswerDTO(

        long Id,
        string QuestionTitle,
        decimal Point,
        ICollection<GetOptionItemInChoiceQuestionDTO>? Options = null)
    {
        public GetQuestionInQuizAnswerDTO():this(0,string.Empty,0,new List<GetOptionItemInChoiceQuestionDTO>()) { }
       
    };
 
}
;