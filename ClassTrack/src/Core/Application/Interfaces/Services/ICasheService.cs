using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface ICasheService
    {
        Task<T> CheckCasheAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration);
    }
}
