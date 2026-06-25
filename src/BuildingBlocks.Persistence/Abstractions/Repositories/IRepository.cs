using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Persistence.Abstractions.Repositories;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TAggregate"></typeparam>
public interface IRepository<TAggregate> : IRepository<TAggregate, Guid> where TAggregate : AggregateRoot<Guid>
{
}