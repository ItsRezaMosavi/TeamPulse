using BuildingBlocks.Application.Abstractions.Behaviors;
using BuildingBlocks.Results;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Application.Behaviors;
/// <summary>
/// Represents a CQRS pipeline behavior that logs the execution lifecycle of a request.
/// </summary>
/// <remarks>
/// This behavior logs when request processing starts, whether it completes successfully
/// or with a failure result, and records any unhandled exceptions thrown during
/// request execution.
/// </remarks>
public class LoggingBehavior<TRequest, TResult>(ILogger<LoggingBehavior<TRequest, TResult>> logger)
    : IPipelineBehavior<TRequest, TResult>
    where TRequest : notnull
{
    private static readonly string _requestName = typeof(TRequest).Name;
    /// <summary>
    /// Logs the execution of the current request before and after it is processed.
    /// </summary>
    /// <param name="request">The incoming request.</param>
    /// <param name="next">
    /// A delegate that represents the next step in the request pipeline.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    /// A <see cref="Result{TResult}"/> representing the outcome of the request.
    /// </returns>
    public async Task<Result<TResult>> HandleAsync(TRequest request, RequestHandlerDelegate<TResult> next,
                                                   CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Handling request {RequestName}.", _requestName);

        try
        {
            var result = await next();

            if (result.IsSuccess)
            {
                logger.LogInformation("Request {RequestName} completed successfully.", _requestName);
            }

            else
            {
                logger.LogWarning("Request {RequestName} completed with failure.", _requestName);
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred while handling request {RequestName}.", _requestName);
            throw;
        }
    }
}