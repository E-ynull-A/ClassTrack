using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        string GetUserId();
        void DeleteCookie(string key);
        string GetUserRole();
        string GetUserEmail();
    }
}
