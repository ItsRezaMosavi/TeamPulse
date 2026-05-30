using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Application.Repositories;

public interface IReadRepository<TAggregate> : IReadRepository<TAggregate, Guid> where TAggregate : AggregateRoot<Guid>
{
}