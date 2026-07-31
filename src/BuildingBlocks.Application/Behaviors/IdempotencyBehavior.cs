using BuildingBlocks.Application.Abstractions.Behaviors;
using BuildingBlocks.Application.Abstractions.Idempotency;
using BuildingBlocks.Application.Context;
using BuildingBlocks.Application.Idempotency.Enums;
using BuildingBlocks.Application.Idempotency.Errors;
using BuildingBlocks.Application.Idempotency.Models;
using BuildingBlocks.Application.Options;
using BuildingBlocks.Results;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Application.Behaviors;

public class IdempotencyBehavior<TRequest, TResult>(
	IIdempotencyStore store,
	IIdempotencySerializer serializer,
	IDateTimeProvider dateTimeProvider,
	IOptions<IdempotencyOptions> options)
	: IPipelineBehavior<TRequest, TResult>
	where TRequest : IIdempotentCommand<TResult>
{
	private readonly IdempotencyOptions _options = options.Value;

	public async Task<Result<TResult>> HandleAsync(TRequest request,
												   RequestHandlerDelegate<TResult> next,
												   CancellationToken cancellationToken = default)
	{
		var key = request.IdempotencyKey;
		var now = dateTimeProvider.UtcNow;
		var expiresAt = now.Add(_options.Expiration);

		var acquireResult = await store.AcquireAsync(key, expiresAt, cancellationToken);

		if (acquireResult.Status != AcquireStatus.Acquired) return HandleExistingRequest(acquireResult);

		return await ExecuteNewRequestAsync(acquireResult.Entry, next, cancellationToken);
	}


	private Result<TResult> HandleExistingRequest(AcquireResult acquireResult)
	{
		if (acquireResult.Status == AcquireStatus.InProgress) return new RequestInProgressError();

		if (string.IsNullOrWhiteSpace(acquireResult.Entry.SerializedResponse))
			throw new InvalidOperationException("Cached idempotency response is missing.");

		return serializer.Deserialize<TResult>(acquireResult.Entry.SerializedResponse!);
	}

	private async Task<Result<TResult>> ExecuteNewRequestAsync(IdempotencyEntry entry,
															   RequestHandlerDelegate<TResult> next,
															   CancellationToken cancellationToken)
	{
		Result<TResult> result;
		try
		{
			result = await next();
		}
		catch
		{
			await store.ReleaseAsync(entry.Key, cancellationToken);
			throw;
		}

		if (result.IsFailure && !_options.CacheFailures)
		{
			await store.ReleaseAsync(entry.Key, cancellationToken);
			return result;
		}

		var serializedResponse = serializer.Serialize(result);

		await store.CompleteAsync(entry.Key, serializedResponse, dateTimeProvider.UtcNow, cancellationToken);
		return result;
	}
}