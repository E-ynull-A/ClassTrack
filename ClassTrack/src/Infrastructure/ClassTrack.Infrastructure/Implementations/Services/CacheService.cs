using ClassTrack.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task SetCasheAsync<T>(string key, T data, TimeSpan? expiration)
        {           
            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            string json = JsonSerializer.Serialize(data);

            await _cache.SetStringAsync(key, json, option);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
           string? json = await _cache.GetStringAsync(key);

            if (json != null)            
               return JsonSerializer.Deserialize<T>(json);
            
            return default;
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
      
    }
}
