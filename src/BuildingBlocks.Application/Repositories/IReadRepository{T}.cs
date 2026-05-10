using System.Linq.Expressions;
using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Application.Repositories;

public interface IReadRepository<TAggregate, in TId> where TAggregate : AggregateRoot<TId>
{
    Task<IReadOnlyList<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TAggregate>> FindAsync(Expression<Func<TAggregate, bool>> predicate,
                                              CancellationToken cancellationToken = default);

    Task<TAggregate?> SingleOrDefaultAsync(Expression<Func<TAggregate, bool>> predicate,
                                           CancellationToken cancellationToken = default);

    Task<TAggregate?> FirstOrDefaultAsync(Expression<Func<TAggregate, bool>> predicate,
                                          CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TAggregate, bool>> predicate,
                        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TAggregate, bool>>? predicate = null,
                         CancellationToken cancellationToken = default);
}