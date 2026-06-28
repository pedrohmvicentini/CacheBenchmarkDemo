using CacheBenchmarkDemo.Application.Abstractions.Benchmarking;
using CacheBenchmarkDemo.Application.Common.Benchmark;
using CacheBenchmarkDemo.Application.Products.Queries.BenchmarkProductRead;
using FluentAssertions;
using Moq;

namespace CacheBenchmarkDemo.UnitTests.Application
{
    public sealed class BenchmarkProductReadQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Execute_All_Strategies_And_Return_Product()
        {
            var productId = Guid.NewGuid();
            var product = new ProductResponse(productId, "Product A", 99.90m);

            var strategy1 = new Mock<IProductReadStrategy>();
            strategy1.SetupGet(x => x.Name).Returns("database");
            strategy1.Setup(x => x.ExecuteAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StrategyExecutionResult<ProductResponse?>(
                    product,
                    false,
                    30,
                    100));

            var strategy2 = new Mock<IProductReadStrategy>();
            strategy2.SetupGet(x => x.Name).Returns("redis");
            strategy2.Setup(x => x.ExecuteAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StrategyExecutionResult<ProductResponse?>(
                    product,
                    true,
                    5,
                    100));

            var handler = new BenchmarkProductReadQueryHandler(new[]
            {
            strategy1.Object,
            strategy2.Object
        });

            var result = await handler.Handle(
                new BenchmarkProductReadQuery(productId),
                CancellationToken.None);

            result.Should().NotBeNull();
            result.Product.Should().NotBeNull();
            result.Product!.Id.Should().Be(productId);
            result.Results.Should().HaveCount(2);
            result.Results.Should().Contain(x => x.Strategy == "database");
            result.Results.Should().Contain(x => x.Strategy == "redis");
        }
    }
}