namespace CacheBenchmarkDemo.Application.Common.Benchmark
{
    public sealed record StrategyExecutionResult<T>(
    T? Data,
    bool FromCache,
    long ElapsedMs,
    int PayloadSizeBytes);
}
