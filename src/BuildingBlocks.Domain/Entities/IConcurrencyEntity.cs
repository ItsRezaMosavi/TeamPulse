namespace BuildingBlocks.Domain.Entities;

public interface IConcurrencyEntity
{
	byte[] RowVersion { get; }
}