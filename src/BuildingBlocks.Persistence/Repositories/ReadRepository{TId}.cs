using BuildingBlocks.Application.Persistence.Repositories;
using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Specification.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Repositories;

/// <summary>
/// 
/// </summary>
/// <param name="dbContext"></param>
/// <param name="evaluator"></param>
/// <typeparam name="TAggregate"></typeparam>
/// <typeparam name="TId"></typeparam>
public abstract class ReadRepository<TAggregate, TId>(DbContext dbContext, ISpecificationEvaluator evaluator)
    : IReadRepository<TAggregate, TId> where TAggregate : AggregateRoot<TId>
{
    private readonly DbSet<TAggregate> _dbSet = dbContext.Set<TAggregate>();

    /// <summary>
    /// 
    /// </summary>
    private IQueryable<TAggregate> Query => _dbSet.AsNoTracking();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await Query.SingleOrDefaultAsync(e => EqualityComparer<TId>.Default.Equals(e.Id, id),
                                                cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<TAggregate>> ListAsync(IQuerySpecification<TAggregate>? specification = null,
                                                           CancellationToken cancellationToken = default)
    {
        IQueryable<TAggregate> query = Query;

        if (specification is not null)
            query = evaluator.GetQuery(query, specification);

        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TAggregate?> SingleOrDefaultAsync(IQuerySpecification<TAggregate> specification,
                                                        CancellationToken cancellationToken = default)
    {
        return await evaluator.GetQuery(Query, specification)
                              .SingleOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TAggregate> SingleAsync(IQuerySpecification<TAggregate> specification,
                                              CancellationToken cancellationToken = default)
    {
        return await evaluator.GetQuery(Query, specification).SingleAsync(cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TAggregate?> FirstOrDefaultAsync(IQuerySpecification<TAggregate> specification,
                                                       CancellationToken cancellationToken = default)
    {
        return await evaluator.GetQuery(Query, specification)
                              .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<TAggregate> FirstAsync(IQuerySpecification<TAggregate> specification,
                                             CancellationToken cancellationToken = default)
    {
        return await evaluator.GetQuery(Query, specification).FirstAsync(cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> AnyAsync(IQuerySpecification<TAggregate> specification,
                                     CancellationToken cancellationToken = default)
    {
        return await evaluator.GetQuery(Query, specification).AnyAsync(cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await Query.AnyAsync(x => EqualityComparer<TId>.Default.Equals(x.Id, id), cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> CountAsync(IQuerySpecification<TAggregate>? specification = null,
                                      CancellationToken cancellationToken = default)
    {
        var query = Query;

        if (specification is not null)
            query = evaluator.GetQuery(query, specification);

        return await query.CountAsync(cancellationToken);
    }
}