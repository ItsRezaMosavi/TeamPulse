using BuildingBlocks.Application.Abstractions.Behaviors;
using BuildingBlocks.Application.Abstractions.CQRS.Commands;
using BuildingBlocks.Persistence.Abstractions;
using BuildingBlocks.Results;

namespace BuildingBlocks.Persistence.Behaviors;

/// <summary>
/// Represents a CQRS pipeline behavior that commits changes through the unit of work
/// after a command has been successfully processed.
/// </summary>
/// <remarks>
/// This behavior ensures that database changes are persisted only when the command
/// handler completes successfully. If the handler returns a failure result or throws
/// an exception, no changes are committed.
/// </remarks>
public class TransactionBehavior<TRequest, TResult>(IUnitOfWork unitOfWork) : IPipelineBehavior<TRequest, TResult>
    where TRequest : ICommand<TResult>
{
    /// <summary>
    /// Executes the next delegate in the pipeline and commits the unit of work
    /// if the request completes successfully.
    /// </summary>
    /// <param name="request">The incoming command.</param>
    /// <param name="next">
    /// A delegate that represents the next step in the request pipeline.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    /// A <see cref="Result{TResult}"/> representing the outcome of the command.
    /// </returns>
    public async Task<Result<TResult>> HandleAsync(TRequest request, RequestHandlerDelegate<TResult> next,
                                                   CancellationToken cancellationToken = default)
    {
        var result = await next();

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}