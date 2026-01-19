using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetQuestionDTO(
        long Id,

        long QuizId,
        string QuizName,

        long ClassId,
        string ClassName,

        string Title,
        decimal Point,

        string Type
        );
    
}
