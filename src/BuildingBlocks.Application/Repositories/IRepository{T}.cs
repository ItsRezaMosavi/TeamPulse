using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Application.Repositories;

public interface IRepository<TAggregate, in TId> : IReadRepository<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>
{
    Task AddAsync(TAggregate entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default);
    void Update(TAggregate entity);
    void UpdateRange(IEnumerable<TAggregate> entities);
    void Remove(TAggregate entity);
    void RemoveRange(IEnumerable<TAggregate> entities);
}