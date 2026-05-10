using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Domain.Repositories;

public interface IRepository<TAggregate> : IRepository<TAggregate, Guid> where TAggregate : AggregateRoot<Guid>
{
}