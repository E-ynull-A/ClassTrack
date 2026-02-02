using ClassTrack.Application.DTOs;
using ClassTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface ITokenService
    {
        AccessTokenDTO CreateAccessToken(AppUser user,
                                         IEnumerable<string> roles,
                                         int minutes);
        RefreshTokenDTO GenerateRefreshToken();
    }
}
