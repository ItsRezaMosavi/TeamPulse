using BuildingBlocks.Application.Repositories;
using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Repositories;

/// <summary>
    /// Base implementation of read-only repository operations for aggregate roots with a default Guid identifier.
    /// </summary>
/// <param name="dbContext"></param>
/// <param name="evaluator"></param>
/// <typeparam name="TAggregate"></typeparam>
public abstract class ReadRepository<TAggregate>(DbContext dbContext, EfSpecificationEvaluator evaluator)
    : ReadRepository<TAggregate, Guid>(dbContext, evaluator), IReadRepository<TAggregate>
    where TAggregate : AggregateRoot
{
}