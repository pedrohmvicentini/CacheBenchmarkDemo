using MediatR;

namespace CacheBenchmarkDemo.Application.Products.Queries.BenchmarkProductRead
{
    public sealed record BenchmarkProductReadQuery(Guid ProductId) : IRequest<BenchmarkProductReadResponse>;
}
