using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.Abstractions.Repositories;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.Extensions;
using BuildingBlocks.Specification.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Repositories;

/// <summary>
/// Base implementation of read-only repository operations for aggregate roots with a generic identifier type.
/// </summary>
/// <param name="dbContext"></param>
/// <param name="evaluator"></param>
/// <typeparam name="TAggregate"></typeparam>
/// <typeparam name="TId"></typeparam>
public abstract class ReadRepository<TAggregate, TId>(
	BuildingBlocksDbContext dbContext,
	ISpecificationEvaluator evaluator)
	: IReadRepository<TAggregate, TId> where TAggregate : AggregateRoot<TId>
{
	private readonly DbSet<TAggregate> _dbSet = dbContext.Set<TAggregate>();

	/// <summary>
	/// Base implementation of read-only repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	private IQueryable<TAggregate> Query => _dbSet.AsNoTracking();

	/// <summary>
	/// Base implementation of read-only repository operations for aggregate roots with a generic identifier type.
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
	/// Base implementation of read-only repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<IReadOnlyList<TAggregate>> ListAsync(IQuerySpecification<TAggregate>? specification = null,
														   CancellationToken cancellationToken = default)
	{
		var query = Query;

		if (specification is not null) query = query.WithSpecification(specification, evaluator);

		return await query.ToListAsync(cancellationToken);
	}

	/// <summary>
	/// Base implementation of read-only repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<TAggregate?> SingleOrDefaultAsync(IQuerySpecification<TAggregate> specification,
														CancellationToken cancellationToken = default)
	{
		return await Query.WithSpecification(specification, evaluator).SingleOrDefaultAsync(cancellationToken);
	}

	/// <summary>
	/// Base implementation of read-only repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<TAggregate> SingleAsync(IQuerySpecification<TAggregate> specification,
											  CancellationToken cancellationToken = default)
	{
		return await Query.WithSpecification(specification, evaluator).SingleAsync(cancellationToken);
	}

	/// <summary>
	/// Base implementation of read-only repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<TAggregate?> FirstOrDefaultAsync(IQuerySpecification<TAggregate> specification,
													   CancellationToken cancellationToken = default)
	{
		return await Query.WithSpecification(specification, evaluator).FirstOrDefaultAsync(cancellationToken);
	}

	/// <summary>
	/// Base implementation of read-only repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public async Task<TAggregate> FirstAsync(IQuerySpecification<TAggregate> specification,
											 CancellationToken cancellationToken = default)
	{
		return await Query.WithSpecification(specification, evaluator).FirstAsync(cancellationToken);
	}

	/// <summary>
	/// Base implementation of read-only repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<bool> AnyAsync(IQuerySpecification<TAggregate> specification,
									 CancellationToken cancellationToken = default)
	{
		return await Query.WithSpecification(specification, evaluator).AnyAsync(cancellationToken);
	}

	/// <summary>
	/// Base implementation of read-only repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
	{
		return await Query.AnyAsync(x => EqualityComparer<TId>.Default.Equals(x.Id, id), cancellationToken);
	}

	/// <summary>
	/// Base implementation of read-only repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<int> CountAsync(IQuerySpecification<TAggregate>? specification = null,
									  CancellationToken cancellationToken = default)
	{
		var query = Query;

		if (specification is not null) query = query.WithSpecification(specification, evaluator);

		return await query.CountAsync(cancellationToken);
	}
}