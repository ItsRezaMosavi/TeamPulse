using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.Abstractions.Repositories;
using BuildingBlocks.Specification.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Repositories;

/// <summary>
    /// Base implementation of repository operations for aggregate roots with a generic identifier type, including write operations.
    /// </summary>
/// <param name="dbContext"></param>
/// <param name="evaluator"></param>
/// <typeparam name="TAggregate"></typeparam>
/// <typeparam name="TId"></typeparam>
public abstract class Repository<TAggregate, TId>(DbContext dbContext, ISpecificationEvaluator evaluator)
    : ReadRepository<TAggregate, TId>(dbContext, evaluator), IRepository<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>
{
    private readonly DbSet<TAggregate> _dbSet = dbContext.Set<TAggregate>();

    /// <summary>
    /// Base implementation of repository operations for aggregate roots with a generic identifier type, including write operations.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    public async Task AddAsync(TAggregate entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Base implementation of repository operations for aggregate roots with a generic identifier type, including write operations.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    public async Task AddRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    /// <summary>
    /// Base implementation of repository operations for aggregate roots with a generic identifier type, including write operations.
    /// </summary>
    /// <param name="entity"></param>
    public void Update(TAggregate entity)
    {
        _dbSet.Update(entity);
    }

    /// <summary>
    /// Base implementation of repository operations for aggregate roots with a generic identifier type, including write operations.
    /// </summary>
    /// <param name="entities"></param>
    public void UpdateRange(IEnumerable<TAggregate> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    /// <summary>
    /// Base implementation of repository operations for aggregate roots with a generic identifier type, including write operations.
    /// </summary>
    /// <param name="entity"></param>
    public void Remove(TAggregate entity)
    {
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// Base implementation of repository operations for aggregate roots with a generic identifier type, including write operations.
    /// </summary>
    /// <param name="entities"></param>
    public void RemoveRange(IEnumerable<TAggregate> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}