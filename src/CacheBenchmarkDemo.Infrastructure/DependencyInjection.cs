using CacheBenchmarkDemo.Application.Abstractions.Benchmarking;
using CacheBenchmarkDemo.Application.Abstractions.Caching;
using CacheBenchmarkDemo.Application.Abstractions.Data;
using CacheBenchmarkDemo.Infrastructure.Caching;
using CacheBenchmarkDemo.Infrastructure.Persistence;
using CacheBenchmarkDemo.Infrastructure.Persistence.Repositories;
using CacheBenchmarkDemo.Infrastructure.ReadStrategies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CacheBenchmarkDemo.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Postgres"));
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "cache-benchmark-demo:";
            });

            services.AddHybridCache(options =>
            {
                options.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(5),
                    LocalCacheExpiration = TimeSpan.FromMinutes(1)
                };
            });

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IDistributedCacheService, DistributedCacheService>();
            services.AddScoped<IHybridCacheReader, HybridCacheReader>();

            services.AddScoped<IProductReadStrategy, DatabaseProductReadStrategy>();
            services.AddScoped<IProductReadStrategy, HybridCacheProductReadStrategy>();
            services.AddScoped<IProductReadStrategy, RedisProductReadStrategy>();

            return services;
        }
    }
}
