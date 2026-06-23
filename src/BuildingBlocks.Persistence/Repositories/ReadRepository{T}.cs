using System.Linq.Expressions;
using BuildingBlocks.Application.Persistence.Repositories;
using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Repositories;

public class ReadRepository<TAggregate, TId>(ApplicationDbContext dbContext)
    : IReadRepository<TAggregate, TId> where TAggregate : AggregateRoot<TId>
{
    private readonly DbSet<TAggregate> _dbSet = dbContext.Set<TAggregate>();

    public async Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<TAggregate>> GetAsync(Expression<Func<TAggregate, bool>>? predicate = null,
                                                          CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TAggregate?> SingleOrDefaultAsync(Expression<Func<TAggregate, bool>> predicate,
                                                        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TAggregate?> FirstOrDefaultAsync(Expression<Func<TAggregate, bool>> predicate,
                                                       CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<TAggregate, bool>> predicate,
                                     CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => EqualityComparer<TId>.Default.Equals(x.Id, id), cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<TAggregate, bool>>? predicate = null,
                                      CancellationToken cancellationToken = default)
    {
        if (predicate is null)
            return await _dbSet.CountAsync(cancellationToken);

        return await _dbSet.CountAsync(predicate, cancellationToken);
    }
}