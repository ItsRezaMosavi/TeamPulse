using BuildingBlocks.Application.Abstractions.CQRS.Commands;

namespace BuildingBlocks.Application.Abstractions.Idempotency;

/// <summary>
/// Represents a command that should be executed only once for a given idempotency key.
/// </summary>
/// <typeparam name="TResult">The type of the command result.</typeparam>
public interface IIdempotentCommand<TResult> : ICommand<TResult>
{
	/// <summary>
	/// Gets the unique idempotency key for this command.
	/// </summary>
	string IdempotencyKey { get; }
}