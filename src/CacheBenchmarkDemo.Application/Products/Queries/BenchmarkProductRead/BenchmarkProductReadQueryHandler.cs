using CacheBenchmarkDemo.Application.Abstractions.Benchmarking;
using MediatR;

namespace CacheBenchmarkDemo.Application.Products.Queries.BenchmarkProductRead
{
    public sealed class BenchmarkProductReadQueryHandler
    : IRequestHandler<BenchmarkProductReadQuery, BenchmarkProductReadResponse>
    {
        private readonly IEnumerable<IProductReadStrategy> _strategies;

        public BenchmarkProductReadQueryHandler(IEnumerable<IProductReadStrategy> strategies)
        {
            _strategies = strategies;
        }

        public async Task<BenchmarkProductReadResponse> Handle(
            BenchmarkProductReadQuery request,
            CancellationToken cancellationToken)
        {
            var orderedStrategies = _strategies
                .OrderBy(x => x.Name)
                .ToList();

            var results = new List<BenchmarkStrategyResult>();
            ProductResponse? finalProduct = null;

            foreach (var strategy in orderedStrategies)
            {
                var result = await strategy.ExecuteAsync(request.ProductId, cancellationToken);

                if (finalProduct is null && result.Data is not null)
                    finalProduct = result.Data;

                results.Add(new BenchmarkStrategyResult(
                    strategy.Name,
                    result.ElapsedMs,
                    result.FromCache,
                    result.PayloadSizeBytes));
            }

            return new BenchmarkProductReadResponse(
                request.ProductId,
                DateTime.UtcNow,
                results,
                finalProduct);
        }
    }
}
