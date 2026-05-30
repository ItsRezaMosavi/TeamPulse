using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Application.Persistence.Repositories;

public interface IReadRepository<TAggregate> : IReadRepository<TAggregate, Guid> where TAggregate : AggregateRoot<Guid>
{
}