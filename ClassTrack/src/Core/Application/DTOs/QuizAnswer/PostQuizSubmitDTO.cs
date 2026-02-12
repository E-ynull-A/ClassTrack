using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Domain.Entities
{
    public record PostQuizSubmitDTO(
        
        long QuestionId,
        long? AnswerId,
        ICollection<long>? AnswerIds,
        string? AnswerText,
        bool IsEvaluated);
   
}
