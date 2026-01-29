using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetQuizAnswerItemDTO(

        long QuestionId,
        string QuestionTitle,
        long StudentQuizId,
        bool IsEvaluated);
  

}
