using BuildingBlocks.Application.Idempotency.Enums;

namespace BuildingBlocks.Application.Idempotency.Models;

public sealed class IdempotencyEntry
{
	private IdempotencyEntry()
	{
	}

	public static IdempotencyEntry Create(string key,
										  IdempotencyStatus status,
										  DateTime createdAtUtc,
										  DateTime expiresAtUtc,
										  DateTime? completedAtUtc = null,
										  string? serializedResponse = null)
	{
		return new IdempotencyEntry
		{
			Key = key,
			Status = status,
			CreatedAtUtc = createdAtUtc,
			ExpiresAtUtc = expiresAtUtc,
			CompletedAtUtc = completedAtUtc,
			SerializedResponse = serializedResponse
		};
	}

	public string Key { get; private set; } = default!;

	public IdempotencyStatus Status { get; private set; }

	public string? SerializedResponse { get; private set; }

	public DateTime CreatedAtUtc { get; private set; }

	public DateTime? CompletedAtUtc { get; private set; }

	public DateTime ExpiresAtUtc { get; private set; }
}