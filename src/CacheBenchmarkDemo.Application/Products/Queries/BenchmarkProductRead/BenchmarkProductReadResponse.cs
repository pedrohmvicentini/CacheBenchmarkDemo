namespace CacheBenchmarkDemo.Application.Products.Queries.BenchmarkProductRead
{
    public sealed record BenchmarkProductReadResponse(
    Guid ProductId,
    DateTime MeasuredAtUtc,
    IReadOnlyCollection<BenchmarkStrategyResult> Results,
    ProductResponse? Product);
}
