using CacheBenchmarkDemo.Application.Common.Benchmark;
using CacheBenchmarkDemo.Application.Products.Queries.BenchmarkProductRead;

namespace CacheBenchmarkDemo.Application.Abstractions.Benchmarking
{
    public interface IProductReadStrategy
    {
        string Name { get; }

        Task<StrategyExecutionResult<ProductResponse?>> ExecuteAsync(
            Guid productId,
            CancellationToken cancellationToken);
    }
}
