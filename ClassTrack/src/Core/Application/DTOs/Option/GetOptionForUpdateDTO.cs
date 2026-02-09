using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record GetOptionForUpdateDTO(
        long Id,
        string Variant,
        bool IsCorrect);
    
}
