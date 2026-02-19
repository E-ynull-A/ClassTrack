using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record ErrorResponseDTO(
        
        int StatusCode,
        string Message,
        string Detail);
   
}
