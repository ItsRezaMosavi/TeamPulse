using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Application.Repositories;

/// <summary>
/// Provides read-only repository operations for aggregate roots with a default Guid identifier.
/// </summary>
/// <typeparam name="TAggregate">The type of the aggregate root.</typeparam>
public interface IReadRepository<TAggregate> : IReadRepository<TAggregate, Guid> where TAggregate : AggregateRoot<Guid>
{
}