namespace CacheBenchmarkDemo.Application.Products.Queries.BenchmarkProductRead
{
    public sealed record ProductResponse(
    Guid Id,
    string Name,
    decimal Price);
}
