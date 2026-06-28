namespace CacheBenchmarkDemo.Application.Common.Cache
{
    public static class CacheKeys
    {
        public static string ProductById(Guid id) => $"products:{id}";
    }
}
