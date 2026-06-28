using CacheBenchmarkDemo.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace CacheBenchmarkDemo.Infrastructure.Persistence
{
    public sealed class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
