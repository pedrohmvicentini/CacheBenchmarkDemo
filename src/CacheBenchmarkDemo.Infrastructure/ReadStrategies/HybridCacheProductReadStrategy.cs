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
    public sealed class HybridCacheProductReadStrategy : IProductReadStrategy
    {
        private readonly IHybridCacheReader _hybridCacheReader;
        private readonly IProductReadRepository _repository;

        public string Name => "hybrid-cache";

        public HybridCacheProductReadStrategy(
            IHybridCacheReader hybridCacheReader,
            IProductReadRepository repository)
        {
            _hybridCacheReader = hybridCacheReader;
            _repository = repository;
        }

        public async Task<StrategyExecutionResult<ProductResponse?>> ExecuteAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            var key = CacheKeys.ProductById(productId);

            var (response, fromCache) = await _hybridCacheReader.GetOrCreateAsync(
                key,
                async ct =>
                {
                    var product = await _repository.GetByIdAsync(productId, ct);

                    return product is null
                        ? null
                        : new ProductResponse(product.Id, product.Name, product.Price);
                },
                expiration: TimeSpan.FromMinutes(5),
                localExpiration: TimeSpan.FromMinutes(1),
                cancellationToken);

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
