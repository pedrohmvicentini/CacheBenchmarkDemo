
# CacheBenchmarkDemo

**In progress**

A study project using **.NET 10** application built to compare different data retrieval strategies and their impact on application performance.

In the end of this project it will have the following benchmarks:
- Direct database query with **PostgreSQL**
- Caching with **HybridCache**
- Caching with **Redis Distributed Cache** 

The project follows **DDD**, **Clean Architecture**, **CQRS**, and **TDD** principles, providing a simple and extensible foundation for benchmarking cache implementations in ASP.NET Core.

## Features

- Direct PostgreSQL query
- HybridCache
- Redis Distributed Cache
- Execution time comparison
- Minimal API
- Entity Framework Core
- Docker support
- Unit and Integration Tests

## Tech Stack

- .NET 10
- ASP.NET Core Minimal API
- C#
- Entity Framework Core
- PostgreSQL
- Redis
- HybridCache
- MediatR
- FluentValidation
- Serilog
- xUnit
- FluentAssertions
- Moq
- Docker & Docker Compose

## Project Structure

```text
src
├── Api
├── Application
├── Contracts
├── Domain
└── Infrastructure

tests
├── UnitTests
└── IntegrationTests
```

## Running the Project

### Requirements

- .NET 10 SDK
- Docker

### Start infrastructure

```bash
cd docker
docker compose up -d
```

### Restore dependencies

```bash
dotnet restore
```

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run --project src/CacheBenchmarkDemo.Api
```

## API

### Benchmark

```http
GET /api/v1/products/{id}/benchmark
```
Use the ID **11111111-1111-1111-1111-111111111111** to test. 

Ex.: 
```http
GET /api/v1/products/11111111-1111-1111-1111-111111111111/benchmark
```

Returns the execution time for:

- Database
- HybridCache
- Redis

## Roadmap

- Benchmark with multiple iterations

---

**This project is intended as a reference implementation for modern .NET backend architecture and caching strategies.**