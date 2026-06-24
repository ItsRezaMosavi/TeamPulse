# TeamPulse Building Blocks

A comprehensive collection of reusable building blocks for .NET 9.0 applications following Domain-Driven Design (DDD) and Clean Architecture principles.

## Overview

This solution provides foundational components for building robust, maintainable, and scalable applications. Each project encapsulates specific concerns and follows best practices for separation of concerns, testability, and extensibility.

**Key Benefits:**
- 🏗️ **Clean Architecture**: Clear separation between domain, application, and infrastructure layers
- 🎯 **DDD Patterns**: Full support for aggregates, entities, value objects, domain events, and domain rules
- ✅ **Type-Safe Error Handling**: Result pattern with strongly-typed errors and HTTP status code mapping
- 📄 **XML Documentation**: All projects generate XML documentation files for IntelliSense support
- 🔧 **Extensible Design**: Abstract base classes and interfaces for easy customization
- 📦 **Ready-to-Use**: Pre-built implementations for common patterns like Unit of Work, Repository, and Specification

---

## Projects

### 📦 BuildingBlocks.Domain

**Purpose:** Core domain primitives and DDD patterns that form the foundation of your domain model.

**Key Components:**

| Component | Description |
|-----------|-------------|
| `AggregateRoot<TId>` | Base class for aggregate roots with domain event support. Manages consistency boundaries and collects domain events for eventual consistency. |
| `AggregateRoot` | Convenience class using `Guid` as the identifier type. |
| `Entity<TId>` | Base class for entities with identity-based equality comparison. Protects against comparing entities with unassigned IDs. |
| `Entity` | Convenience class using `Guid` as the identifier type. |
| `AuditableEntity<TId, TUserId>` | Entity with automatic audit tracking (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy). |
| `AuditableEntity` | Convenience class using `Guid` for both entity and user ID types. |
| `ISoftDeletable<TUserId>` | Interface for soft-delete pattern with Delete/Restore methods and audit fields (DeletedAt, DeletedBy, IsDeleted). |
| `ValueObject` | Abstract base class for value objects with proper equality semantics based on component properties. |
| `IDomainRule` | Interface for domain rule validation pattern with Code, Message, and IsBroken() method. |
| `DomainEventBase` | Base class for domain events providing unique EventId and OccurredOn timestamp. |
| `IDomainEvent` | Marker interface for domain events. |
| `IHasDomainEvents` | Interface for aggregates that can raise domain events. |
| `DomainRuleException` | Exception thrown when a domain rule is violated. |

**Usage Example:**
```csharp
public class Order : AggregateRoot<Guid>
{
    public void Complete()
    {
        // Apply business logic
        CheckRule(new OrderMustBePendingRule(Status));
        
        Status = OrderStatus.Completed;
        AddDomainEvent(new OrderCompletedEvent(Id));
    }
}
```

---

### 📦 BuildingBlocks.Application

**Purpose:** Application layer abstractions defining contracts for infrastructure implementations, cross-cutting concerns, and request handling patterns.

**Key Components:**

#### Persistence Interfaces
| Interface | Description |
|-----------|-------------|
| `IUnitOfWork` | Unit of Work pattern for managing database transactions and coordinating SaveChanges. |
| `IRepository<TAggregate, TId>` | Write repository interface with CRUD operations (Add, Update, Remove). |
| `IRepository<TAggregate>` | Convenience interface using `Guid` identifiers. |
| `IReadRepository<TAggregate, TId>` | Read-only repository interface with query methods (GetById, Get, SingleOrDefault, Any, Count). |
| `IReadRepository<TAggregate>` | Convenience interface using `Guid` identifiers. |

#### Abstractions
| Interface | Description |
|-----------|-------------|
| `IGuidGenerator` | Abstraction for GUID generation (testable alternative to Guid.NewGuid()). |
| `IClock` | Abstraction for DateTime operations (testable alternative to DateTime.Now/UtcNow). |
| `IEventDispatcher` | Interface for dispatching domain events after persistence. |

#### Request Patterns
| Interface | Description |
|-----------|-------------|
| `ICommand` | Marker interface for command requests (no return value). |
| `ICommand<TResult>` | Marker interface for command requests returning a result. |
| `IQuery<TResult>` | Marker interface for query requests returning a result. |

#### Pipeline Behaviors
| Interface | Description |
|-----------|-------------|
| `ILoggingBehavior` | Marker interface for logging pipeline behavior. |
| `ITransactionBehavior` | Marker interface for transaction pipeline behavior. |
| `IValidationBehavior` | Marker interface for validation pipeline behavior. |

#### Context Interfaces
| Interface | Description |
|-----------|-------------|
| `ICurrentUser` | Interface for accessing current user information. |
| `IDateTimeProvider` | Interface for date/time operations in application context. |
| `IRequestContext` | Interface for accessing request-specific context. |

---

### 📦 BuildingBlocks.Infrastructure

**Purpose:** Concrete implementations of application layer abstractions and infrastructure services.

**Key Components:**

| Component | Description |
|-----------|-------------|
| `DependencyInjection` | Extension methods for registering infrastructure services (IClock, IGuidGenerator). |
| `SystemClock` | Implementation of IClock returning system DateTime. |
| `DefaultGuidGenerator` | Standard GUID generator using Guid.NewGuid(). |
| `SequentialGuidGenerator` | Optimized GUID generator creating sequential GUIDs for better database index performance. |
| `MediatorEventDispatcher` | Domain event dispatcher using MediatR for publishing events. |

**Registration Example:**
```csharp
// Default registration
services.AddBuildingBlocksInfrastructure();

// With sequential GUIDs for better SQL Server performance
services.AddSequentialGuidGenerator();
```

---

### 📦 BuildingBlocks.Persistence

**Purpose:** Entity Framework Core implementations for repositories, unit of work, and database context with outbox pattern support.

**Key Components:**

| Component | Description |
|-----------|-------------|
| `ApplicationDbContext` | Base EF Core DbContext with OutboxMessages DbSet for the outbox pattern. |
| `UnitOfWork` | Implementation of IUnitOfWork that captures domain events before SaveChanges and dispatches them after. |
| `Repository<TAggregate, TId>` | EF Core repository implementation with full CRUD operations. |
| `ReadRepository<TAggregate, TId>` | EF Core read-only repository with specification pattern support. |
| `EfSpecificationEvaluator` | Evaluates specifications and applies criteria, ordering, includes, and paging to queries. |
| `OutboxMessage` | Entity for storing domain events for reliable delivery (outbox pattern). |
| `OutboxMessageConfiguration` | EF Core configuration for OutboxMessage table. |
| `DependencyInjection` | Extension methods for registering persistence services. |

**Features:**
- ✅ Automatic domain event capture and dispatch
- ✅ Specification pattern support for complex queries
- ✅ Outbox pattern for reliable event delivery
- ✅ Configurable schema and table names

**Registration Example:**
```csharp
services.AddBuildingBlocksPersistence(options =>
    options.UseSqlServer(connectionString));
```

---

### 📦 BuildingBlocks.Pagination

**Purpose:** Type-safe pagination with built-in validation and comprehensive metadata.

**Key Components:**

| Component | Description |
|-----------|-------------|
| `PageRequest` | Immutable value object representing pagination parameters (Page, PageSize, Skip). Includes validation for page bounds. |
| `PagedResult<T>` | Immutable result containing paginated items and metadata (TotalCount, TotalPages, HasNextPage, HasPreviousPage, IsEmpty). |

**Features:**
- ✅ Default page size: 10 items
- ✅ Maximum page size: 100 items (configurable)
- ✅ 1-based page indexing
- ✅ Automatic Skip calculation
- ✅ Validation for negative/zero values
- ✅ Metadata for UI pagination controls

**Usage Example:**
```csharp
// Create validated page request
var pageRequest = PageRequest.Create(page: 1, pageSize: 25);

// After querying data
var pagedResult = PagedResult<User>.Create(users, pageRequest, totalCount);

// Access metadata
Console.WriteLine($"Page {pagedResult.Page} of {pagedResult.TotalPages}");
Console.WriteLine($"Total Items: {pagedResult.TotalCount}");
Console.WriteLine($"Has Next: {pagedResult.HasNextPage}");
```

---

### 📦 BuildingBlocks.Results

**Purpose:** Functional error handling using the Result pattern with strongly-typed errors and HTTP status code mapping.

**Key Components:**

#### Core Types
| Type | Description |
|------|-------------|
| `Result` | Non-generic result indicating success/failure with optional errors. |
| `Result<T>` | Generic result containing a value on success or errors on failure. |
| `ResultBase` | Abstract base class with IsSuccess, IsFailure, and Errors properties. |
| `Error` | Error representation with Type, Code, and Message. |
| `ErrorType` | Enum defining error categories (Failure, Validation, NotFound, Conflict, Unauthorized, Forbidden, BusinessRule, TooManyRequests, Unavailable, Timeout). |

#### Predefined Error Classes
| Class | HTTP Status | Use Case |
|-------|-------------|----------|
| `FailureError` | 500 | General server failures |
| `ValidationError` | 400 | Input validation failures |
| `NotFoundError` | 404 | Resource not found |
| `ConflictError` | 409 | Data conflicts (duplicate keys, etc.) |
| `UnauthorizedError` | 401 | Authentication required |
| `ForbiddenError` | 403 | Access denied |
| `BusinessRuleError` | 422 | Domain/business rule violations |
| `TooManyRequestsError` | 429 | Rate limiting |
| `UnavailableError` | 503 | Service unavailable |
| `TimeoutError` | 504 | Request timeout |

#### Extensions
| Extension | Description |
|-----------|-------------|
| `AsHttpStatusCode()` | Converts ErrorType to corresponding HttpStatusCode enum value. |

**Usage Examples:**
```csharp
// Success case
public async Task<Result<User>> GetUserAsync(Guid id)
{
    var user = await _repository.GetByIdAsync(id);
    return user != null ? Result<User>.Success(user) : new NotFoundError("User not found");
}

// Multiple errors
public async Task<Result> DeleteUserAsync(Guid id)
{
    var errors = new List<Error>();
    
    if (!await _repository.ExistsAsync(id))
        errors.Add(new NotFoundError());
    
    if (await _repository.HasRelatedOrdersAsync(id))
        errors.Add(new ConflictError("User has related orders"));
    
    return errors.Count > 0 ? Result.Failure(errors.ToArray()) : Result.Success();
}

// HTTP status code mapping
var statusCode = error.Type.AsHttpStatusCode(); // Returns HttpStatusCode.NotFound
```

---

### 📦 BuildingBlocks.Specification

**Purpose:** Specification pattern implementation for encapsulating query logic and enabling composable, reusable query definitions.

**Key Components:**

#### Core Interfaces
| Interface | Description |
|-----------|-------------|
| `ISpecification<T>` | Base specification interface with ToExpression() and IsSatisfiedBy() methods. |
| `IQuerySpecification<T>` | Query-specific specification with Criteria, OrderBy, Includes, Skip, Take, and IsPagingEnabled. |
| `ISpecificationEvaluator` | Interface for applying specifications to IQueryable sources. |

#### Base Classes
| Class | Description |
|-------|-------------|
| `Specification<T>` | Abstract base class for domain specifications with And/Or/Not composition operators. |
| `QuerySpecification<T>` | Base class for EF Core query specifications with paging and include support. |

#### Composition Specifications
| Class | Description |
|-------|-------------|
| `AndSpecification<T>` | Combines two specifications with logical AND. |
| `OrSpecification<T>` | Combines two specifications with logical OR. |
| `NotSpecification<T>` | Negates a specification. |

#### Helpers
| Class | Description |
|-------|-------------|
| `ReplaceParameterVisitor` | Expression visitor for combining specification expressions. |

**Usage Example:**
```csharp
public class ActiveUsersSpecification : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression()
    {
        return u => u.IsActive && !u.IsDeleted;
    }
}

public class UsersByEmailDomainSpecification : QuerySpecification<User>
{
    public UsersByEmailDomainSpecification(string domain, int page, int pageSize)
    {
        ApplyPaging((page - 1) * pageSize, pageSize);
        ApplyOrderBy(u => u.CreatedAt, isAscending: false);
        Include.Add(u => u.Profile);
        
        Criteria = u => u.Email.EndsWith(domain);
    }
}

// Composition
var spec = new ActiveUsersSpecification().And(new PremiumUsersSpecification());
var users = await _repository.GetAsync(spec);
```

---

## Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- IDE with C# support (Visual Studio, Rider, or VS Code)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/ItsRezaMosavi/TeamPulse.git
   cd TeamPulse
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore BuildingBlocks.sln
   ```

3. **Build the solution:**
   ```bash
   dotnet build BuildingBlocks.sln
   ```

4. **Run tests (if available):**
   ```bash
   dotnet test BuildingBlocks.sln
   ```

---

## Architecture

The solution follows Clean Architecture and Domain-Driven Design principles:

```
BuildingBlocks.sln
│
├── BuildingBlocks.Domain          # Domain layer - Entities, Aggregates, Value Objects, Domain Events
│   ├── Aggregates/                # Aggregate root base classes
│   ├── Entities/                  # Entity base classes (Entity, AuditableEntity, ISoftDeletable)
│   ├── ValueObjects/              # Value object base class
│   ├── Events/                    # Domain event interfaces and base classes
│   ├── Rules/                     # Domain rule interface
│   └── Exceptions/                # Domain-specific exceptions
│
├── BuildingBlocks.Application     # Application layer - Interfaces, DTOs, Behaviors
│   ├── Persistence/               # Repository and UnitOfWork interfaces
│   ├── Requests/                  # Command and Query interfaces
│   ├── Behaviors/                 # Pipeline behavior interfaces
│   ├── Abstractions/              # Service abstractions (IClock, IGuidGenerator)
│   ├── Events/                    # Event dispatcher interface
│   └── Context/                   # Context interfaces (ICurrentUser, IRequestContext)
│
├── BuildingBlocks.Infrastructure  # Infrastructure implementations
│   ├── Identifiers/               # GUID generators (Default, Sequential)
│   ├── Time/                      # Clock implementations
│   ├── Events/                    # Event dispatcher using MediatR
│   └── DependencyInjection.cs     # Service registration extensions
│
├── BuildingBlocks.Persistence     # EF Core implementations
│   ├── DbContexts/                # ApplicationDbContext
│   ├── Repositories/              # Repository implementations
│   ├── Configurations/            # Entity configurations
│   ├── Outbox/                    # Outbox pattern entities
│   ├── Specifications/            # Specification evaluator
│   └── DependencyInjection.cs     # Service registration extensions
│
├── BuildingBlocks.Pagination      # Pagination support
│   ├── PageRequest.cs             # Pagination request value object
│   └── PagedResult.cs             # Paginated result with metadata
│
├── BuildingBlocks.Results         # Result pattern and error handling
│   ├── Errors/                    # Predefined error classes
│   ├── Defaults/                  # Default messages and codes
│   ├── Extensions/                # Extension methods
│   ├── Error.cs                   # Error base class
│   ├── ErrorType.cs               # Error type enumeration
│   ├── Result.cs                  # Non-generic result
│   ├── Result{T}.cs               # Generic result
│   └── ResultBase.cs              # Result base class
│
└── BuildingBlocks.Specification   # Specification pattern
    ├── Base/                      # Base specification classes
    ├── Contracts/                 # Specification interfaces
    ├── Composition/               # And/Or/Not specifications
    └── Helpers/                   # Expression visitors
```

---

## Key Features

### Domain-Driven Design
- **Aggregates**: Full aggregate root support with domain event collection
- **Entities**: Identity-based equality with protection against unassigned IDs
- **Value Objects**: Proper equality semantics based on component properties
- **Domain Events**: Event sourcing ready with unique IDs and timestamps
- **Domain Rules**: Rule-based validation pattern for business invariants

### Error Handling
- **Result Pattern**: Functional approach to error handling without exceptions
- **Typed Errors**: Strongly-typed error classes for each error category
- **HTTP Mapping**: Automatic conversion to HTTP status codes
- **Composability**: Support for multiple errors in a single result

### Data Access
- **Repository Pattern**: Generic repositories with specification support
- **Unit of Work**: Transaction management with automatic event dispatch
- **Outbox Pattern**: Reliable domain event delivery
- **Soft Delete**: Built-in support for soft-delete pattern
- **Audit Tracking**: Automatic CreatedAt/CreatedBy/UpdatedAt/UpdatedBy tracking

### Query & Pagination
- **Specification Pattern**: Composable, reusable query definitions
- **Type-Safe Pagination**: Validated page requests with metadata
- **EF Core Integration**: Seamless integration with Entity Framework Core

### Extensibility
- **Interface-Based**: All infrastructure behind abstractions
- **Dependency Injection**: Ready-made DI registration extensions
- **Testable**: Clock and GUID generator abstractions for unit testing

---

## Design Principles

1. **Separation of Concerns**: Each project has a single responsibility
2. **Dependency Inversion**: High-level modules depend on abstractions
3. **Immutability**: Value objects and results are immutable
4. **Validation Early**: Input validation at construction/factory methods
5. **Explicit Dependencies**: All dependencies expressed through constructors
6. **Testability**: Abstractions enable easy mocking and unit testing

---

## Best Practices

### Using Domain Rules
```csharp
public class CreateUserHandler
{
    public async Task<Result<Guid>> Handle(CreateUserCommand command)
    {
        // Validate business rules before creating
        CheckRule(new EmailMustBeUniqueRule(command.Email));
        CheckRule(new UserAgeMustBeValidRule(command.DateOfBirth));
        
        var user = new User(command.Email, command.Name);
        await _repository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        
        return Result<Guid>.Success(user.Id);
    }
}
```

### Implementing Soft Delete
```csharp
public class Product : AggregateRoot<Guid>, ISoftDeletable<Guid>
{
    public DateTime? DeletedAt { get; protected set; }
    public Guid? DeletedBy { get; protected set; }
    public bool IsDeleted { get; protected set; }
    
    // ISoftDeletable methods are implemented explicitly or via base class
}
```

### Using Specifications
```csharp
public class RecentActiveUsersSpec : QuerySpecification<User>
{
    public RecentActiveUsersSpec(DateTime cutoffDate)
    {
        Criteria = u => u.LastLoginAt >= cutoffDate && u.IsActive;
        ApplyOrderBy(u => u.LastLoginAt, isAscending: false);
        ApplyPaging(0, 50);
        Include.Add(u => u.Roles);
    }
}
```

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## Contributing

Contributions are welcome! Please follow these guidelines:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Commit** your changes (`git commit -m 'Add amazing feature'`)
4. **Push** to the branch (`git push origin feature/amazing-feature`)
5. **Open** a Pull Request

### Code Style
- Follow existing code conventions
- Add XML documentation comments for public members
- Include unit tests for new features
- Ensure all existing tests pass

---

## Support

For issues, questions, or suggestions:
- 🐛 **Bug Reports**: Open an issue on GitHub
- 💡 **Feature Requests**: Open an issue with the "enhancement" label
- 📧 **Questions**: Use GitHub Discussions

---

## Acknowledgments

This library draws inspiration from:
- Domain-Driven Design by Eric Evans
- Clean Architecture by Robert C. Martin
- The Specification Pattern community
- The Result Pattern functional programming community
