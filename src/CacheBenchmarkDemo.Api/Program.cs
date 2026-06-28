using CacheBenchmarkDemo.Api.Endpoints;
using CacheBenchmarkDemo.Application;
using CacheBenchmarkDemo.Infrastructure;
using CacheBenchmarkDemo.Infrastructure.Persistence;
using MediatR;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
    config.WriteTo.Console();
});

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CacheBenchmarkDemo.Application.DependencyInjection).Assembly);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSerilogRequestLogging();

app.MapProductEndpoints();

await ApplyMigrationsAndSeedAsync(app);

app.Run();

static async Task ApplyMigrationsAndSeedAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await dbContext.Database.EnsureCreatedAsync();
    await AppDbContextSeed.SeedAsync(dbContext);
}

public partial class Program;