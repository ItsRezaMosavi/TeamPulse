using BuildingBlocks.Domain.Aggregates;

namespace BuildingBlocks.Application.Repositories;

public interface IRepository<TAggregate> : IRepository<TAggregate, Guid> where TAggregate : AggregateRoot<Guid>
{
}