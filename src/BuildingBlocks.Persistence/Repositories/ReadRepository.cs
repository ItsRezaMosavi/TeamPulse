using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.DbContexts;

namespace BuildingBlocks.Persistence.Repositories;

public class ReadRepository<TAggregate>(ApplicationDbContext dbContext)
    : ReadRepository<TAggregate, Guid>(dbContext) where TAggregate : AggregateRoot
{
}