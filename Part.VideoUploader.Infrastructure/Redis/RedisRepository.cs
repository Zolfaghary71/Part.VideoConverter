using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Part.VideoUploader.Service.Contracts.Infrastructure;

namespace Part.VideoUploader.Infrastructure.Redis
{
    public class RedisRepository<T> : IRedisRepository<T> where T : class
    {
        private readonly IDistributedCache _cache;

        public RedisRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> Get(Guid id)
        {
            var cachedData = await _cache.GetAsync(id.ToString());
            if (cachedData == null)
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(cachedData);
        }
        

        public async Task<bool> Set(Guid id, T @object)
        {
            var jsonData = JsonSerializer.SerializeToUtf8Bytes(@object);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            await _cache.SetAsync(id.ToString(), jsonData, options);
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await _cache.RemoveAsync(id.ToString());
            return true;
        }
    }
}