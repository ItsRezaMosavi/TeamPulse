using BuildingBlocks.Application.Abstractions.Behaviors;
using BuildingBlocks.Results;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Application.Behaviors;

public class LoggingBehavior<TRequest, TResult>(ILogger<LoggingBehavior<TRequest, TResult>> logger)
    : IPipelineBehavior<TRequest, TResult>
    where TRequest : notnull
{
    private static readonly string _requestName = typeof(TRequest).Name;

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