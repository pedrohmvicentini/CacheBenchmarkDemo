using CacheBenchmarkDemo.Application.Products.Queries.BenchmarkProductRead;
using MediatR;

namespace CacheBenchmarkDemo.Api.Endpoints
{
    public static class ProductEndpoints
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/products")
                .WithTags("Products");

            group.MapGet("/{id:guid}/benchmark", async (
                    Guid id,
                    ISender sender,
                    CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new BenchmarkProductReadQuery(id), cancellationToken);

                if (result.Product is null)
                    return Results.NotFound(new
                    {
                        Message = $"Product {id} not found."
                    });

                return Results.Ok(result);
            })
                .WithName("BenchmarkProductRead")
                .WithSummary("Executa benchmark de leitura do produto")
                .WithDescription("Compara leitura direta no banco, HybridCache e Redis.")
                .Produces<BenchmarkProductReadResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return app;
        }
    }
}
