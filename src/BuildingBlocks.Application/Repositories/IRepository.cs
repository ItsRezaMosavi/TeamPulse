using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Application.Repositories;

/// <summary>
/// Provides repository operations for aggregate roots with a default Guid identifier, including write operations.
/// </summary>
/// <typeparam name="TAggregate">The type of the aggregate root.</typeparam>
public interface IRepository<TAggregate> : IRepository<TAggregate, Guid> where TAggregate : AggregateRoot<Guid>
{
}