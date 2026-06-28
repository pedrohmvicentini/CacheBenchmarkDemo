namespace CacheBenchmarkDemo.Application.Abstractions.Data
{
    public interface IProductReadRepository
    {
        Task<ProductReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }

    public sealed record ProductReadModel(Guid Id, string Name, decimal Price);
}
