using BuildingBlocks.Domain.Entities.Entities;

namespace BuildingBlocks.Domain.Entities.ConcurrencyEntities;

public class ConcurrencyEntity<TId> : Entity<TId>, IConcurrencyEntity
{
	public byte[] RowVersion { get; } = [];
}