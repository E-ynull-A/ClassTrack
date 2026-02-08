using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record PostQuizDTO(

       string Name,
       DateTime StartTime,
       double Duration,
       long ClassRoomId,

       ICollection<PostChoiceQuestionDTO>? ChoiceQuestions,
       ICollection<PostOpenQuestionDTO>? OpenQuestions);
    
}
