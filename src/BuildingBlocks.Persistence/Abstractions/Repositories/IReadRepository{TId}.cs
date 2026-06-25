using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Specification.Contracts;

namespace BuildingBlocks.Persistence.Abstractions.Repositories;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TAggregate"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IReadRepository<TAggregate, in TId> where TAggregate : AggregateRoot<TId>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<TAggregate>> ListAsync(IQuerySpecification<TAggregate>? specification = null,
                                              CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TAggregate?> SingleOrDefaultAsync(IQuerySpecification<TAggregate> specification,
                                           CancellationToken cancellationToken = default);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TAggregate> SingleAsync(IQuerySpecification<TAggregate> specification,
                                 CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TAggregate?> FirstOrDefaultAsync(IQuerySpecification<TAggregate> specification,
                                          CancellationToken cancellationToken = default);


    Task<TAggregate> FirstAsync(IQuerySpecification<TAggregate> specification,
                                CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(IQuerySpecification<TAggregate> specification,
                        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountAsync(IQuerySpecification<TAggregate>? specification = null,
                         CancellationToken cancellationToken = default);
}