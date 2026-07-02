using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Application.Repositories;

/// <summary>
/// Provides repository operations for aggregate roots with a generic identifier type, including write operations.
/// </summary>
/// <typeparam name="TAggregate">The type of the aggregate root.</typeparam>
/// <typeparam name="TId">The type of the aggregate root's identifier.</typeparam>
public interface IRepository<TAggregate, in TId> : IReadRepository<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>
{
    /// <summary>
    /// Adds a new aggregate root to the repository.
    /// </summary>
    /// <param name="entity">The aggregate root to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task AddAsync(TAggregate entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Adds multiple aggregate roots to the repository.
    /// </summary>
    /// <param name="entities">The collection of aggregate roots to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task AddRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing aggregate root in the repository.
    /// </summary>
    /// <param name="entity">The aggregate root to update.</param>
    void Update(TAggregate entity);
    
    /// <summary>
    /// Updates multiple aggregate roots in the repository.
    /// </summary>
    /// <param name="entities">The collection of aggregate roots to update.</param>
    void UpdateRange(IEnumerable<TAggregate> entities);
    
    /// <summary>
    /// Removes an aggregate root from the repository.
    /// </summary>
    /// <param name="entity">The aggregate root to remove.</param>
    void Remove(TAggregate entity);
    
    /// <summary>
    /// Removes multiple aggregate roots from the repository.
    /// </summary>
    /// <param name="entities">The collection of aggregate roots to remove.</param>
    void RemoveRange(IEnumerable<TAggregate> entities);
}