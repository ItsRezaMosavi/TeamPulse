using BuildingBlocks.Application.Persistence.Repositories;
using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Repositories;

public abstract class ReadRepository<TAggregate>(DbContext dbContext, EfSpecificationEvaluator evaluator)
    : ReadRepository<TAggregate, Guid>(dbContext, evaluator), IReadRepository<TAggregate>
    where TAggregate : AggregateRoot
{
}