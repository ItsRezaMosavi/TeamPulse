using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Persistence.Abstractions.Repositories;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TAggregate"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IRepository<TAggregate, in TId> : IReadRepository<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddAsync(TAggregate entity, CancellationToken cancellationToken = default);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    void Update(TAggregate entity);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entities"></param>
    void UpdateRange(IEnumerable<TAggregate> entities);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    void Remove(TAggregate entity);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entities"></param>
    void RemoveRange(IEnumerable<TAggregate> entities);
}