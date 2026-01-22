using ClassTrack.Domain;
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

        long ClassRoomId,
        string ClassRoomName,

        string Title,
        decimal Point,
        string QuestionType

        ) : 
        GetQuestionDTO (Id, QuizId, QuizName, ClassRoomId
                        ,ClassRoomName, Title, Point, QuestionType)
    {
        public GetOpenQuestionDTO() : this(0, 0, "", 0, "", "", 0, string.Empty) { }
    };
    
}
