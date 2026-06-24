using BuildingBlocks.Application.Persistence.Repositories;
using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Repositories;

public abstract class Repository<TAggregate>(DbContext dbContext, EfSpecificationEvaluator evaluator)
    : Repository<TAggregate, Guid>(dbContext, evaluator), IRepository<TAggregate>
    where TAggregate : AggregateRoot
{
}