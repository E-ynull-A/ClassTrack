using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetChoiceQuestionForUpdateDTO(
        long Id,
        string Title,
        decimal Point,
        bool IsMultiple,
        ICollection<GetOptionForUpdateDTO> Options);


}
