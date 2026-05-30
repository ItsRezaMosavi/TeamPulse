using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Infrastructure.Persistence.Repositories;

public class ReadRepository<TAggregate>(ApplicationDbContext dbContext)
    : ReadRepository<TAggregate, Guid>(dbContext) where TAggregate : AggregateRoot
{
}