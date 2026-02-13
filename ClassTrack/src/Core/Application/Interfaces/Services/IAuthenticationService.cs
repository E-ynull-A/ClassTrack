using ClassTrack.Application.DTOs;
using Microsoft.AspNetCore.Authentication.BearerToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task RegisterAsync(RegisterDTO registerDTO);
        Task<ResponseTokenDTO> LoginAsync(LoginDTO loginDTO);
        Task LogoutAsync();
        Task ResetPasswordAsync(ResetPasswordDTO passwordDTO);
    }
}
