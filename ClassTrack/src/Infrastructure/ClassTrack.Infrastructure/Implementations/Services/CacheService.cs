using ClassTrack.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class CacheService:ICasheService
    {
        private readonly IMemoryCache _cache;
        private static readonly SemaphoreSlim _lock = new(1, 1); 

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> CheckCasheAsync<T>(string key,
                                                Func<Task<T>> factory,
                                                TimeSpan? expiration)
        {

            if (_cache.TryGetValue(key, out T result)) return result;

            await _lock.WaitAsync();

            try
            {
                if (_cache.TryGetValue(key, out result)) return result;

                result = await factory();

                var option = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
                };

                _cache.Set(key, result, option);

                return result;
            }

            finally
            {
                _lock.Release();
            }  
        }

    }
}
