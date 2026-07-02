using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Specification.Contracts;

namespace BuildingBlocks.Application.Repositories;

/// <summary>
/// Provides read-only repository operations for aggregate roots with a generic identifier type.
/// </summary>
/// <typeparam name="TAggregate">The type of the aggregate root.</typeparam>
/// <typeparam name="TId">The type of the aggregate root's identifier.</typeparam>
public interface IReadRepository<TAggregate, in TId> where TAggregate : AggregateRoot<TId>
{
    /// <summary>
    /// Retrieves an aggregate root by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the aggregate root to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The aggregate root if found; otherwise, null.</returns>
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of aggregate roots based on an optional specification.
    /// </summary>
    /// <param name="specification">The specification to filter the results, or null to retrieve all.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A read-only list of aggregate roots.</returns>
    Task<IReadOnlyList<TAggregate>> ListAsync(IQuerySpecification<TAggregate>? specification = null,
                                              CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single aggregate root that matches the specification, or null if not found.
    /// </summary>
    /// <param name="specification">The specification to match.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The matching aggregate root if found; otherwise, null.</returns>
    Task<TAggregate?> SingleOrDefaultAsync(IQuerySpecification<TAggregate> specification,
                                           CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a single aggregate root that matches the specification.
    /// </summary>
    /// <param name="specification">The specification to match.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The matching aggregate root.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition.</exception>
    Task<TAggregate> SingleAsync(IQuerySpecification<TAggregate> specification,
                                 CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the first aggregate root that matches the specification, or null if not found.
    /// </summary>
    /// <param name="specification">The specification to match.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The first matching aggregate root if found; otherwise, null.</returns>
    Task<TAggregate?> FirstOrDefaultAsync(IQuerySpecification<TAggregate> specification,
                                          CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves the first aggregate root that matches the specification.
    /// </summary>
    /// <param name="specification">The specification to match.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The first matching aggregate root.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no element satisfies the condition.</exception>
    Task<TAggregate> FirstAsync(IQuerySpecification<TAggregate> specification,
                                CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether any aggregate root matches the specification.
    /// </summary>
    /// <param name="specification">The specification to match.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if any aggregate root matches; otherwise, false.</returns>
    Task<bool> AnyAsync(IQuerySpecification<TAggregate> specification,
                        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether an aggregate root with the specified identifier exists.
    /// </summary>
    /// <param name="id">The identifier to check.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>True if an aggregate root with the specified ID exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the number of aggregate roots based on an optional specification.
    /// </summary>
    /// <param name="specification">The specification to filter the results, or null to count all.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The number of aggregate roots.</returns>
    Task<int> CountAsync(IQuerySpecification<TAggregate>? specification = null,
                         CancellationToken cancellationToken = default);
}