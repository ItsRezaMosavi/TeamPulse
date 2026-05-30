# TeamPulse Building Blocks

A collection of reusable building blocks for .NET 9.0 applications following Domain-Driven Design (DDD) principles.

## Overview

This solution provides foundational components for building robust, maintainable applications with clean architecture. The building blocks include:

- **Domain Layer**: Core DDD patterns including aggregates, entities, value objects, and domain rules
- **Application Layer**: Application services, repositories, and unit of work patterns
- **Infrastructure Layer**: Persistence and infrastructure implementations
- **Pagination**: Flexible pagination support with validation
- **Results**: Result pattern implementation for error handling

## Projects

### BuildingBlocks.Domain
Core domain primitives and patterns:
- `AggregateRoot<TId>` - Base class for aggregate roots with domain event support
- `Entity<TId>` - Base class for entities with value-based equality
- `AuditableEntity<TId, TUserId>` - Entity with audit tracking (CreatedAt, UpdatedAt, DeletedAt, etc.)
- `ValueObject` - Base class for value objects with proper equality semantics
- `IDomainRule` / `DomainPolicy` - Domain rule validation pattern
- `Clause` - Validation result representation
- `DomainEventBase` / `IDomainEvent` - Domain events with unique IDs and timestamps

### BuildingBlocks.Application
Application layer abstractions:
- `IUnitOfWork` - Unit of Work pattern for transaction management
- `IRepository<TAggregate>` - Repository pattern for aggregate persistence
- `IReadRepository<TAggregate>` - Read-only repository interface

### BuildingBlocks.Infrastructure
Infrastructure implementations (details in project).

### BuildingBlocks.Pagination
Pagination support with built-in validation:
- `PageRequest` - Pagination request with page number and page size
- `PagedResult<T>` - Paginated result with metadata (total count, total pages, has next/previous)
- Built-in validation policies for page requests and results

### BuildingBlocks.Results
Result pattern for functional error handling:
- `Result` / `Result<T>` - Success/failure result types
- `Error` - Error representation with type, code, and message
- `ErrorType` enum: Failure, Validation, NotFound, Conflict, Unauthorized, Forbidden, BusinessRule, TooManyRequests, Unavailable, Timeout
- Predefined error classes: `ValidationError`, `NotFoundError`, `ConflictError`, `BusinessRuleError`, etc.
- HTTP status code extensions

## Getting Started

### Prerequisites
- .NET 9.0 SDK or later

### Installation

1. Clone the repository:
```bash
git clone https://github.com/ItsRezaMosavi/TeamPulse.git
cd TeamPulse
```

2. Restore dependencies:
```bash
dotnet restore TeamPulse.sln
```

3. Build the solution:
```bash
dotnet build TeamPulse.sln
```

## Usage Examples

### Domain Rules Pattern

```csharp
public sealed class PageMustBeGreaterThanZeroRule(int page) : IDomainRule
{
    public string Code => "PAGE_MUST_BE_GREATER_THAN_ZERO";

    public Clause Evaluate()
    {
        if (page > 0)
            return Clause.Valid();
        return Clause.Invalid("Page must be greater than zero", ("Page", page));
    }
}
```

### Aggregate Root with Domain Events

```csharp
public class Order : AggregateRoot<Guid>
{
    public void Complete()
    {
        // Business logic
        AddDomainEvent(new OrderCompletedEvent(Id));
    }
}
```

### Result Pattern

```csharp
public async Task<Result<User>> GetUserAsync(Guid id)
{
    var user = await _repository.GetByIdAsync(id);
    
    if (user is null)
        return new NotFoundError("User not found");
    
    return Result<User>.Success(user);
}
```

### Pagination

```csharp
var pageRequest = PageRequest.Create(page: 1, pageSize: 20);
var pagedResult = PagedResult<User>.Create(users, pageRequest, totalCount);

// Access pagination metadata
Console.WriteLine($"Total Pages: {pagedResult.TotalPages}");
Console.WriteLine($"Has Next: {pagedResult.HasNextPage}");
```

## Architecture

The solution follows Clean Architecture principles:

```
TeamPulse.sln
├── BuildingBlocks.Domain          # Core domain patterns
├── BuildingBlocks.Application     # Application layer abstractions
├── BuildingBlocks.Infrastructure  # Infrastructure implementations
├── BuildingBlocks.Pagination      # Pagination support
└── BuildingBlocks.Results         # Result pattern & error handling
```

## Key Features

- **Domain-Driven Design**: Full support for DDD patterns including aggregates, entities, value objects, and domain events
- **Validation**: Rule-based validation with `DomainPolicy` and `IDomainRule`
- **Error Handling**: Comprehensive result pattern with typed errors
- **Pagination**: Type-safe pagination with validation
- **Audit Tracking**: Built-in audit properties for entities
- **Soft Delete**: Support for soft-delete pattern via `ISoftDeletable`

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
