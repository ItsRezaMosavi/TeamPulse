using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Domain.Repositories;

public interface IRepository<TAggregate, in TId> where TAggregate : AggregateRoot<TId>
{
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task AddAsync(TAggregate entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default);
    void Update(TAggregate entity);
    void UpdateRange(IEnumerable<TAggregate> entities);
    void Remove(TAggregate entity);
    void RemoveRange(IEnumerable<TAggregate> entities);
}