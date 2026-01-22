using ClassTrack.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record PutChoiceQuestionDTO(
       string Title,
       decimal Point,
       bool IsMultiple,
       ICollection<PutOptionInChoiceQuestionDTO>? Options = null):IBasePutQuestion;

}
