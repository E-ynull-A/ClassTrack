using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface ICacheService
    {
        Task SetCasheAsync<T>(string key,T data, TimeSpan? expiration);
        Task<T?> GetAsync<T>(string key);
        Task RemoveAsync(string key);

    }
}
