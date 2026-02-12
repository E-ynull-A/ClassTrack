using ClassTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record PostQuizAnswerDTO(
        
        long QuizId,
        ICollection<PostQuizSubmitDTO> Answers);
   
}
