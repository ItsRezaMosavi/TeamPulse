using BuildingBlocks.Application.Idempotency.Models;

namespace BuildingBlocks.Application.Abstractions.Idempotency;

public interface IIdempotencyStore
{
	Task<AcquireResult> AcquireAsync(string key,
									 DateTime expiresAtUtc,
									 CancellationToken cancellationToken = default);

	Task CompleteAsync(string key,
					   string serializedResponse,
					   DateTime completedAtUtc,
					   CancellationToken cancellationToken = default);

	Task ReleaseAsync(string key,
					  CancellationToken cancellationToken = default);

	Task CleanUpAsync(CancellationToken cancellationToken = default);
}