using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record PutOptionInChoiceQuestionDTO(
        long? Id,
        string Variant,
        bool IsCorrect,
        bool IsDeleted = false);
    
}
