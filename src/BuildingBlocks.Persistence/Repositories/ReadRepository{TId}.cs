using BuildingBlocks.Application.Persistence.Repositories;
using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.Specifications;
using BuildingBlocks.Specification.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Repositories;

public abstract class ReadRepository<TAggregate, TId>(DbContext dbContext, EfSpecificationEvaluator evaluator)
    : IReadRepository<TAggregate, TId> where TAggregate : AggregateRoot<TId>
{
    private readonly IQueryable<TAggregate> _dbSet = dbContext.Set<TAggregate>().AsNoTracking();


    public async Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.SingleOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
    }

    public async Task<IReadOnlyList<TAggregate>> GetAsync(IQuerySpecification<TAggregate>? specification = null,
                                                          CancellationToken cancellationToken = default)
    {
        IQueryable<TAggregate> query = _dbSet;

        if (specification is not null)
            query = evaluator.GetQuery(query, specification, cancellationToken);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TAggregate?> SingleOrDefaultAsync(IQuerySpecification<TAggregate> specification,
                                                        CancellationToken cancellationToken = default)
    {
        return await evaluator.GetQuery(_dbSet, specification, cancellationToken)
                              .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<TAggregate?> FirstOrDefaultAsync(IQuerySpecification<TAggregate> specification,
                                                       CancellationToken cancellationToken = default)
    {
        return await evaluator.GetQuery(_dbSet, specification, cancellationToken)
                              .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(IQuerySpecification<TAggregate> specification,
                                     CancellationToken cancellationToken = default)
    {
        return await evaluator.GetQuery(_dbSet, specification, cancellationToken).AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.Id!.Equals(id), cancellationToken);
    }

    public async Task<int> CountAsync(IQuerySpecification<TAggregate>? specification = null,
                                      CancellationToken cancellationToken = default)
    {
        var query = _dbSet;

        if (specification is not null)
            query = evaluator.GetQuery(query, specification, cancellationToken);

        return await query.CountAsync(cancellationToken);
    }
}