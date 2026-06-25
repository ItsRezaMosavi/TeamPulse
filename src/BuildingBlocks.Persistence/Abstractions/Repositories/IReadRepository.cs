using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Persistence.Abstractions.Repositories;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TAggregate"></typeparam>
public interface IReadRepository<TAggregate> : IReadRepository<TAggregate, Guid> where TAggregate : AggregateRoot<Guid>
{
}