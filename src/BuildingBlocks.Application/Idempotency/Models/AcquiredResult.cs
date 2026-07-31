using BuildingBlocks.Application.Idempotency.Enums;

namespace BuildingBlocks.Application.Idempotency.Models;

public sealed class AcquireResult
{
	public AcquireStatus Status { get; init; }

	public required IdempotencyEntry Entry { get; init; }
}