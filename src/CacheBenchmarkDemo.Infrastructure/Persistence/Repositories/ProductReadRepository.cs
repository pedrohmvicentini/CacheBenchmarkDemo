using CacheBenchmarkDemo.Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace CacheBenchmarkDemo.Infrastructure.Persistence.Repositories
{
    public sealed class ProductReadRepository(AppDbContext dbContext) : IProductReadRepository
    {
        public async Task<ProductReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await dbContext.Products
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new ProductReadModel(x.Id, x.Name, x.Price))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
