using CacheBenchmarkDemo.Domain.Common;
using CacheBenchmarkDemo.Domain.Products;
using FluentAssertions;

namespace CacheBenchmarkDemo.UnitTests.Domain
{
    public sealed class ProductTests
    {
        [Fact]
        public void Constructor_Should_Create_Product_When_Data_Is_Valid()
        {
            var product = new Product(
                Guid.NewGuid(),
                "Notebook",
                1000m,
                DateTime.UtcNow);

            product.Name.Should().Be("Notebook");
            product.Price.Should().Be(1000m);
        }

        [Fact]
        public void Constructor_Should_Throw_When_Name_Is_Empty()
        {
            var action = () => new Product(
                Guid.NewGuid(),
                "",
                1000m,
                DateTime.UtcNow);

            action.Should().Throw<DomainException>()
                .WithMessage("Product name is required.");
        }

        [Fact]
        public void Constructor_Should_Throw_When_Price_Is_Negative()
        {
            var action = () => new Product(
                Guid.NewGuid(),
                "Notebook",
                -1m,
                DateTime.UtcNow);

            action.Should().Throw<DomainException>()
                .WithMessage("Product price cannot be negative.");
        }
    }
}
