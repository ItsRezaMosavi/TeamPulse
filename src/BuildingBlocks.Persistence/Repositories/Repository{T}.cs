using BuildingBlocks.Application.Persistence.Repositories;
using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Repositories;

public class Repository<TAggregate, TId>(ApplicationDbContext dbContext)
    : ReadRepository<TAggregate, TId>(dbContext), IRepository<TAggregate, TId> where TAggregate : AggregateRoot<TId>
{
    private readonly DbSet<TAggregate> _dbSet = dbContext.Set<TAggregate>();

    public async Task AddAsync(TAggregate entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(TAggregate entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<TAggregate> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void Remove(TAggregate entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TAggregate> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}