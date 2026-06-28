using CacheBenchmarkDemo.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CacheBenchmarkDemo.Infrastructure.Caching
{
    public sealed class DistributedCacheService : IDistributedCacheService
    {
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

        private readonly IDistributedCache _distributedCache;

        public DistributedCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            var json = await _distributedCache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonSerializer.Deserialize<T>(json, SerializerOptions);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken)
        {
            var json = JsonSerializer.Serialize(value, SerializerOptions);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl
            };

            await _distributedCache.SetStringAsync(key, json, options, cancellationToken);
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken)
            => _distributedCache.RemoveAsync(key, cancellationToken);
    }
}
