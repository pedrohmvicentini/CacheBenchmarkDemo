using CacheBenchmarkDemo.Domain.Common;

namespace CacheBenchmarkDemo.Domain.Products
{
    public sealed class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public DateTime UpdatedAtUtc { get; private set; }

        private Product()
        {
        }

        public Product(Guid id, string name, decimal price, DateTime updatedAtUtc)
        {
            if (id == Guid.Empty)
                throw new DomainException("Product id is required.");

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name is required.");

            if (price < 0)
                throw new DomainException("Product price cannot be negative.");

            Id = id;
            Name = name.Trim();
            Price = price;
            UpdatedAtUtc = updatedAtUtc;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new DomainException("Product price cannot be negative.");

            Price = newPrice;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
