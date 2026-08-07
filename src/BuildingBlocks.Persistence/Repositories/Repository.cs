using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.Abstractions.Repositories;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.Specifications;

namespace BuildingBlocks.Persistence.Repositories;

/// <summary>
/// Base implementation of repository operations for aggregate roots with a default Guid identifier, including write operations.
/// </summary>
/// <param name="dbContext"></param>
/// <param name="evaluator"></param>
/// <typeparam name="TAggregate"></typeparam>
public abstract class Repository<TAggregate>(BuildingBlocksDbContext dbContext, EfSpecificationEvaluator evaluator)
	: Repository<TAggregate, Guid>(dbContext, evaluator), IRepository<TAggregate>
	where TAggregate : AggregateRoot
{
}