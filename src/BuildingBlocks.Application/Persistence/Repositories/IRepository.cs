using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Application.Persistence.Repositories;

public interface IRepository<TAggregate> : IRepository<TAggregate, Guid> where TAggregate : AggregateRoot<Guid>
{
}