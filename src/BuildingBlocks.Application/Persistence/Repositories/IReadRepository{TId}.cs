using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Specification.Contracts;

namespace BuildingBlocks.Application.Persistence.Repositories;

public interface IReadRepository<TAggregate, in TId> where TAggregate : AggregateRoot<TId>
{
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TAggregate>> GetAsync(IQuerySpecification<TAggregate>? specification = null,
                                             CancellationToken cancellationToken = default);

    Task<TAggregate?> SingleOrDefaultAsync(IQuerySpecification<TAggregate> specification,
                                           CancellationToken cancellationToken = default);

    Task<TAggregate?> FirstOrDefaultAsync(IQuerySpecification<TAggregate> specification,
                                          CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(IQuerySpecification<TAggregate> specification,
                        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    Task<int> CountAsync(IQuerySpecification<TAggregate>? specification = null,
                         CancellationToken cancellationToken = default);
}