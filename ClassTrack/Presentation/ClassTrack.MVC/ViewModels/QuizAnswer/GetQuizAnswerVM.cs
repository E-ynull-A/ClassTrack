using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.MVC.ViewModels
{
    public record GetQuizAnswerVM(

        long Id,
        long StudentQuizId,
        GetQuestionInQuizAnswerVM Question, 
        long? AnswerId,
        ICollection<long>? AnswerIds,
        string? AnswerText,
        bool IsEvaluated);
    
}
