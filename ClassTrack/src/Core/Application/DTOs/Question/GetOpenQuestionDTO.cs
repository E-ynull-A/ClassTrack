using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetOpenQuestionDTO(
        long Id,

        long QuizId,
        string QuizName,

        long ClassId,
        string ClassName,

        string Title,
        decimal Point,
        string Type

        ) : 
        GetQuestionDTO (Id, QuizId, QuizName, ClassId
                        ,ClassName, Title, Point, Type);
    
}
