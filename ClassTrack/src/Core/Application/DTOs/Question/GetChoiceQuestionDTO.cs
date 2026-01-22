using ClassTrack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetChoiceQuestionDTO(
        long Id,

        bool IsMultiple,

        long QuizId,
        string QuizName,

        long ClassRoomId,
        string ClassRoomName,
        string Title,
        decimal Point,
        string QuestionType,
        ICollection<GetOptionItemInChoiceQuestionDTO>? Options = null) :

        GetQuestionDTO(Id, QuizId, QuizName, ClassRoomId
                       , ClassRoomName, Title, Point, QuestionType)
    {
        public GetChoiceQuestionDTO() : this(0, false, 0, string.Empty, 0, string.Empty, string.Empty, 0, string.Empty, new List<GetOptionItemInChoiceQuestionDTO>()) { }

    };

}
