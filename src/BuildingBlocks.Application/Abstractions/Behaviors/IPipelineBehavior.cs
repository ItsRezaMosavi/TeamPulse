using BuildingBlocks.Results;

namespace BuildingBlocks.Application.Abstractions.Behaviors;

/// <summary>
/// Defines a pipeline behavior that can execute logic before and/or after
/// a request handler within the CQRS processing pipeline.
/// </summary>
/// <typeparam name="TRequest">
/// The type of request being processed.
/// </typeparam>
/// <typeparam name="TResult">
/// The type of value returned by the request handler.
/// </typeparam>
public interface IPipelineBehavior<in TRequest, TResult> where TRequest : notnull
{
    /// <summary>
    /// Processes the specified request and optionally invokes the next
    /// delegate in the pipeline.
    /// </summary>
    /// <param name="request">
    /// The request being processed.
    /// </param>
    /// <param name="next">
    /// A delegate that invokes the next behavior or the final request handler.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to observe cancellation requests.
    /// </param>
    /// <returns>
    /// A <see cref="Result{TResult}"/> containing the outcome of the request.
    /// </returns>
    Task<Result<TResult>> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken = default);
}