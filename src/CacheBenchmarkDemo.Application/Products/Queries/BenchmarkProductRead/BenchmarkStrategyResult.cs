namespace CacheBenchmarkDemo.Application.Products.Queries.BenchmarkProductRead
{
    public sealed record BenchmarkStrategyResult(
    string Strategy,
    long ElapsedMs,
    bool FromCache,
    int PayloadSizeBytes);
}
