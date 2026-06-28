using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CacheBenchmarkDemo.IntegrationTests
{
    public sealed class BenchmarkEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BenchmarkEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetBenchmark_Should_Return_Success_Or_NotFound()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(
                "/api/v1/products/11111111-1111-1111-1111-111111111111/benchmark");

            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
        }
    }
}
