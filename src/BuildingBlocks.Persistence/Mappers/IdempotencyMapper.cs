using BuildingBlocks.Application.Idempotency.Models;
using BuildingBlocks.Persistence.Idempotency;

namespace BuildingBlocks.Persistence.Mappers;

public static class IdempotencyMapper
{
	public static IdempotencyEntry ToEntry(this IdempotencyRecord record)
	{
		return IdempotencyEntry.Create(
									   record.Key,
									   record.Status,
									   record.CreatedAtUtc,
									   record.ExpiresAtUtc,
									   record.CompletedAtUtc,
									   record.SerializedResponse);
	}
}