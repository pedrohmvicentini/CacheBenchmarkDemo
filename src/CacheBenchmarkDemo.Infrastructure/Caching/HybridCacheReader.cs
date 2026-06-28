using CacheBenchmarkDemo.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Hybrid;

namespace CacheBenchmarkDemo.Infrastructure.Caching
{
    public sealed class HybridCacheReader : IHybridCacheReader
    {
        private readonly HybridCache _hybridCache;

        public HybridCacheReader(HybridCache hybridCache)
        {
            _hybridCache = hybridCache;
        }

        public async Task<(T? Value, bool FromCache)> GetOrCreateAsync<T>(
            string key,
            Func<CancellationToken, Task<T?>> factory,
            TimeSpan expiration,
            TimeSpan localExpiration,
            CancellationToken cancellationToken)
        {
            var cacheMiss = false;

            var options = new HybridCacheEntryOptions
            {
                Expiration = expiration,
                LocalCacheExpiration = localExpiration
            };

            var value = await _hybridCache.GetOrCreateAsync(
                key,
                async token =>
                {
                    cacheMiss = true;
                    return await factory(token);
                },
                options,
                cancellationToken: cancellationToken);

            return (value, !cacheMiss);
        }
    }
}
