using BuildingBlocks.Application.Abstractions.Idempotency;
using BuildingBlocks.Application.Context;
using BuildingBlocks.Application.Idempotency.Enums;
using BuildingBlocks.Application.Idempotency.Models;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Idempotency;

/// <summary>
/// Entity Framework Core implementation of <see cref="IIdempotencyStore"/>.
/// </summary>
/// <remarks>
/// This implementation persists idempotency records in the database and coordinates
/// concurrent requests using a combination of:
/// <list type="bullet">
/// <item>
/// <description>
/// A unique index on the idempotency key to prevent duplicate record creation.
/// </description>
/// </item>
/// <item>
/// <description>
/// EF Core optimistic concurrency (<c>RowVersion</c>) to ensure that only one
/// request can reacquire an expired record.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Request lifecycle:
/// </para>
/// <list type="number">
/// <item>
/// <description>
/// A new request acquires ownership by creating an <c>InProgress</c> record.
/// </description>
/// </item>
/// <item>
/// <description>
/// Concurrent requests receive either <c>InProgress</c> or the previously cached
/// completed response.
/// </description>
/// </item>
/// <item>
/// <description>
/// After successful execution, the serialized response is stored and the record
/// is marked as <c>Completed</c>.
/// </description>
/// </item>
/// <item>
/// <description>
/// If execution fails (or failures are not configured to be cached), the record
/// is released, allowing future requests to execute normally.
/// </description>
/// </item>
/// <item>
/// <description>
/// Expired records can be reacquired safely using optimistic concurrency.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// This implementation is resilient to race conditions such as:
/// </para>
/// <list type="bullet">
/// <item><description>Multiple concurrent requests creating the same key.</description></item>
/// <item><description>Concurrent reacquisition of expired records.</description></item>
/// <item><description>Duplicate inserts caused by simultaneous requests.</description></item>
/// </list>
/// </remarks>
public class EfCoreIdempotencyStore(BuildingBlocksDbContext dbContext, IDateTimeProvider dateTimeProvider)
	: IIdempotencyStore
{
	private readonly DbSet<IdempotencyRecord> _records = dbContext.Set<IdempotencyRecord>();

	/// <summary>
	/// Attempts to acquire ownership of an idempotency key.
	/// </summary>
	/// <param name="key">The unique idempotency key.</param>
	/// <param name="expiresAtUtc">
	/// The expiration time assigned if the request is successfully acquired.
	/// </param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>
	/// An <see cref="AcquireResult"/> describing whether the caller acquired the
	/// request, another request is currently processing it, or a cached response
	/// already exists.
	/// </returns>
	/// <remarks>
	/// This method is safe for concurrent execution across multiple application
	/// instances. It relies on a unique database constraint and optimistic
	/// concurrency to guarantee that only one request owns a given idempotency key
	/// at a time.
	/// </remarks>
	public async Task<AcquireResult> AcquireAsync(string key,
												  DateTime expiresAtUtc,
												  CancellationToken cancellationToken = default)
	{
		var record = await _records.SingleOrDefaultAsync(i => i.Key == key, cancellationToken);

		var now = dateTimeProvider.UtcNow;

		if (record is null)
		{
			return await CreateNewAsync(key, expiresAtUtc, now, cancellationToken);
		}

		if (record.Status is IdempotencyStatus.InProgress && !record.IsExpired(now))
		{
			return InProgress(record);
		}

		return await TryAcquireExistingAsync(record, expiresAtUtc, now, cancellationToken);
	}

	/// <summary>
	/// Marks an idempotent request as successfully completed.
	/// </summary>
	/// <param name="key">The idempotency key.</param>
	/// <param name="serializedResponse">
	/// The serialized response that should be returned for subsequent requests
	/// using the same idempotency key.
	/// </param>
	/// <param name="completedAtUtc">
	/// The UTC timestamp indicating when the request completed.
	/// </param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <remarks>
	/// After a request is completed, subsequent requests with the same key will
	/// receive the cached response until the record expires.
	/// </remarks>
	public async Task CompleteAsync(string key,
									string serializedResponse,
									DateTime completedAtUtc,
									CancellationToken cancellationToken = default)
	{
		var record = await _records.SingleOrDefaultAsync(i => i.Key == key, cancellationToken);

		if (record is null) throw new InvalidOperationException($"Record with key {key} not found");

		record.MarkAsComplete(serializedResponse, completedAtUtc);
		await dbContext.SaveChangesAsync(cancellationToken);
	}

	/// <summary>
	/// Releases an idempotency key by removing its associated record.
	/// </summary>
	/// <param name="key">The idempotency key to release.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <remarks>
	/// This method is typically called when request execution fails or when failed
	/// responses are not configured to be cached. Once released, a future request
	/// using the same key is allowed to execute normally.
	/// </remarks>
	public async Task ReleaseAsync(string key, CancellationToken cancellationToken = default)
	{
		var record = _records.SingleOrDefault(i => i.Key == key);

		if (record is null) return;

		_records.Remove(record);
		await dbContext.SaveChangesAsync(cancellationToken);
	}

	/// <summary>
	/// Removes expired idempotency records from the underlying store.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <remarks>
	/// This operation deletes records whose expiration time has elapsed,
	/// preventing unbounded growth of the idempotency table. It is intended to be
	/// executed periodically by a background cleanup process.
	/// </remarks>
	public async Task CleanUpAsync(CancellationToken cancellationToken = default)
	{
		var now = dateTimeProvider.UtcNow;
		await _records.Where(r => r.ExpiresAtUtc <= now).ExecuteDeleteAsync(cancellationToken);
	}

	private async Task<AcquireResult> TryAcquireExistingAsync(IdempotencyRecord record,
															  DateTime expiresAtUtc,
															  DateTime now,
															  CancellationToken cancellationToken = default)
	{
		if (record.Status is IdempotencyStatus.Completed && !record.IsExpired(now)) return Completed(record);

		while (true)
		{
			record.Reacquire(now, expiresAtUtc);

			try
			{
				await dbContext.SaveChangesAsync(cancellationToken);
				return Acquired(record);
			}
			catch (DbUpdateConcurrencyException)
			{
				await dbContext.Entry(record).ReloadAsync(cancellationToken);

				if (record.Status == IdempotencyStatus.Completed && !record.IsExpired(now)) return Completed(record);

				if (record.Status == IdempotencyStatus.InProgress && !record.IsExpired(now)) return InProgress(record);
			}
		}
	}

	private async Task<AcquireResult> CreateNewAsync(string key,
													 DateTime expiresAtUtc,
													 DateTime now,
													 CancellationToken cancellationToken = default)
	{
		var record = IdempotencyRecord.Create(key, IdempotencyStatus.InProgress, now, expiresAtUtc);

		dbContext.Set<IdempotencyRecord>().Add(record);
		try
		{
			await dbContext.SaveChangesAsync(cancellationToken);
		}
		catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
		{
			record = await _records.SingleAsync(i => i.Key == key, cancellationToken);

			if (record.Status is IdempotencyStatus.InProgress && !record.IsExpired(now)) return InProgress(record);

			return await TryAcquireExistingAsync(record, expiresAtUtc, now, cancellationToken);
		}

		return Acquired(record);
	}


	private static AcquireResult Acquired(IdempotencyRecord record) =>
		new() { Status = AcquireStatus.Acquired, Entry = record.ToEntry() };

	private static AcquireResult Completed(IdempotencyRecord record) =>
		new() { Status = AcquireStatus.Completed, Entry = record.ToEntry() };

	private static AcquireResult InProgress(IdempotencyRecord record) =>
		new() { Status = AcquireStatus.InProgress, Entry = record.ToEntry() };
}