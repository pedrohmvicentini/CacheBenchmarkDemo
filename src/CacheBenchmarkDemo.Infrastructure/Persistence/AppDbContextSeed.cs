using CacheBenchmarkDemo.Domain.Products;

namespace CacheBenchmarkDemo.Infrastructure.Persistence
{
    public static class AppDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken = default)
        {
            if (context.Products.Any())
                return;

            var now = DateTime.UtcNow;

            var products = new[]
            {
                new Product(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Product 1", 99.90m, now),
                new Product(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Product 2", 149.50m, now),
                new Product(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Product 3", 250.00m, now),
            };

            await context.Products.AddRangeAsync(products, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
