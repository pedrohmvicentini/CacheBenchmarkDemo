using CacheBenchmarkDemo.Application.Abstractions.Benchmarking;
using CacheBenchmarkDemo.Application.Abstractions.Data;
using CacheBenchmarkDemo.Application.Common.Benchmark;
using CacheBenchmarkDemo.Application.Products.Queries.BenchmarkProductRead;
using System.Diagnostics;
using System.Text.Json;

namespace CacheBenchmarkDemo.Infrastructure.ReadStrategies
{
    public sealed class DatabaseProductReadStrategy(IProductReadRepository repository) : IProductReadStrategy
    {
        public string Name => "database";

        public async Task<StrategyExecutionResult<ProductResponse?>> ExecuteAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            var product = await repository.GetByIdAsync(productId, cancellationToken);

            stopwatch.Stop();

            var response = product is null
                ? null
                : new ProductResponse(product.Id, product.Name, product.Price);

            return new StrategyExecutionResult<ProductResponse?>(
                response,
                FromCache: false,
                ElapsedMs: stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes: EstimatePayloadSize(response));
        }

        private static int EstimatePayloadSize(object? data)
            => data is null ? 0 : JsonSerializer.SerializeToUtf8Bytes(data).Length;
    }
}
