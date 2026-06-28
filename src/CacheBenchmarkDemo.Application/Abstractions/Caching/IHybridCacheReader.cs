namespace CacheBenchmarkDemo.Application.Abstractions.Caching
{
    public interface IHybridCacheReader
    {
        Task<(T? Value, bool FromCache)> GetOrCreateAsync<T>(
            string key,
            Func<CancellationToken, Task<T?>> factory,
            TimeSpan expiration,
            TimeSpan localExpiration,
            CancellationToken cancellationToken);
    }
}
