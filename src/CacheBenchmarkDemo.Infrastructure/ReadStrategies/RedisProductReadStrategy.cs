using CacheBenchmarkDemo.Application.Abstractions.Benchmarking;
using CacheBenchmarkDemo.Application.Abstractions.Caching;
using CacheBenchmarkDemo.Application.Abstractions.Data;
using CacheBenchmarkDemo.Application.Common.Benchmark;
using CacheBenchmarkDemo.Application.Common.Cache;
using CacheBenchmarkDemo.Application.Products.Queries.BenchmarkProductRead;
using System.Diagnostics;
using System.Text.Json;

namespace CacheBenchmarkDemo.Infrastructure.ReadStrategies
{
    public sealed class RedisProductReadStrategy : IProductReadStrategy
    {
        private readonly IDistributedCacheService _cache;
        private readonly IProductReadRepository _repository;

        public string Name => "redis";

        public RedisProductReadStrategy(
            IDistributedCacheService cache,
            IProductReadRepository repository)
        {
            _cache = cache;
            _repository = repository;
        }

        public async Task<StrategyExecutionResult<ProductResponse?>> ExecuteAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            var key = CacheKeys.ProductById(productId);

            var fromCache = true;

            var response = await _cache.GetAsync<ProductResponse>(key, cancellationToken);

            if (response is null)
            {
                fromCache = false;

                var product = await _repository.GetByIdAsync(productId, cancellationToken);

                response = product is null
                    ? null
                    : new ProductResponse(product.Id, product.Name, product.Price);

                if (response is not null)
                {
                    await _cache.SetAsync(key, response, TimeSpan.FromMinutes(5), cancellationToken);
                }
            }

            stopwatch.Stop();

            return new StrategyExecutionResult<ProductResponse?>(
                response,
                fromCache,
                stopwatch.ElapsedMilliseconds,
                EstimatePayloadSize(response));
        }

        private static int EstimatePayloadSize(object? data)
            => data is null ? 0 : JsonSerializer.SerializeToUtf8Bytes(data).Length;
    }
}
