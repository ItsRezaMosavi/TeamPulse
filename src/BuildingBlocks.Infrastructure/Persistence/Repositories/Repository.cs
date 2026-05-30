using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Infrastructure.Persistence.Repositories;

public class Repository<TAggregate>(ApplicationDbContext dbContext)
    : Repository<TAggregate, Guid>(dbContext) where TAggregate : AggregateRoot
{
}