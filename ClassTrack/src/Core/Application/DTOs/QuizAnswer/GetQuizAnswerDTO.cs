using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetQuizAnswerDTO(

        long Id,
        long StudentQuizId,
        GetQuestionInQuizAnswerDTO Question, 
        long? AnswerId,
        string? AnswerText,
        bool IsEvaluated);
    
}
