using ClassTrack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.DTOs
{
    public record RegisterDTO
        (string Name,
        string Surname,
        string UserName,
        string Email,
        string Password,
        string ConfirmPassword,
        byte Age,DateOnly BirthDate,
        UserRole UserRole);
}  
                               
                              