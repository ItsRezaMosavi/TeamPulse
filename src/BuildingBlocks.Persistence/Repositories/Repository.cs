using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.DbContexts;

namespace BuildingBlocks.Persistence.Repositories;

public class Repository<TAggregate>(ApplicationDbContext dbContext)
    : Repository<TAggregate, Guid>(dbContext) where TAggregate : AggregateRoot
{
}