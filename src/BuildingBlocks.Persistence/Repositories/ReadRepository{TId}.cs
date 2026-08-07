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
	/// Base implementation of repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
	{
		return await _dbSet.FindAsync([id], cancellationToken);
	}

	/// <summary>
	/// Base implementation of repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<IReadOnlyList<TAggregate>> ListAsync(IQuerySpecification<TAggregate>? specification = null,
														   CancellationToken cancellationToken = default)
	{
		var query = _dbSet.AsQueryable();

		if (specification is not null) query = query.WithSpecification(specification, evaluator);

		return await query.ToListAsync(cancellationToken);
	}

	public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(IQuerySpecification<TAggregate, TResult> specification,
																 CancellationToken cancellationToken = default)
	{
		var query = _dbSet.AsQueryable();

		var result = query.WithSpecification(specification, evaluator);

		return await result.ToListAsync(cancellationToken);
	}

	/// <summary>
	/// Base implementation of repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<TAggregate?> SingleOrDefaultAsync(IQuerySpecification<TAggregate> specification,
														CancellationToken cancellationToken = default)
	{
		return await _dbSet.WithSpecification(specification, evaluator).SingleOrDefaultAsync(cancellationToken);
	}

	public async Task<TResult?> SingleOrDefaultAsync<TResult>(IQuerySpecification<TAggregate, TResult> specification,
															  CancellationToken cancellationToken = default)
	{
		return await _dbSet.WithSpecification(specification, evaluator).SingleOrDefaultAsync(cancellationToken);
	}

	/// <summary>
	/// Base implementation of repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<TAggregate> SingleAsync(IQuerySpecification<TAggregate> specification,
											  CancellationToken cancellationToken = default)
	{
		return await _dbSet.WithSpecification(specification, evaluator).SingleAsync(cancellationToken);
	}

	public async Task<TResult> SingleAsync<TResult>(IQuerySpecification<TAggregate, TResult> specification,
													CancellationToken cancellationToken = default)
	{
		return await _dbSet.WithSpecification(specification, evaluator).SingleAsync(cancellationToken);
	}

	/// <summary>
	/// Base implementation of repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<TAggregate?> FirstOrDefaultAsync(IQuerySpecification<TAggregate> specification,
													   CancellationToken cancellationToken = default)
	{
		return await _dbSet.WithSpecification(specification, evaluator).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<TResult?> FirstOrDefaultAsync<TResult>(IQuerySpecification<TAggregate, TResult> specification,
															 CancellationToken cancellationToken = default)
	{
		return await _dbSet.WithSpecification(specification, evaluator).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<TResult> FirstAsync<TResult>(IQuerySpecification<TAggregate, TResult> specification,
												   CancellationToken cancellationToken = default)
	{
		return await _dbSet.WithSpecification(specification, evaluator).FirstAsync(cancellationToken);
	}

	/// <summary>
	/// Base implementation of repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<bool> AnyAsync(IQuerySpecification<TAggregate> specification,
									 CancellationToken cancellationToken = default)
	{
		return await _dbSet.WithSpecification(specification, evaluator).AnyAsync(cancellationToken);
	}

	/// <summary>
	/// Base implementation of repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
	{
		return await _dbSet.AnyAsync(x => x.Id!.Equals(id), cancellationToken);
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
		var query = _dbSet.AsQueryable();

		if (specification is not null) query = query.WithSpecification(specification, evaluator);

		return await query.CountAsync(cancellationToken);
	}

	/// <summary>
	///     Base implementation of repository operations for aggregate roots with a generic identifier type.
	/// </summary>
	/// <param name="specification"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public async Task<TAggregate> FirstAsync(IQuerySpecification<TAggregate> specification,
											 CancellationToken cancellationToken = default)
	{
		return await _dbSet.WithSpecification(specification, evaluator).FirstAsync(cancellationToken);
	}
}