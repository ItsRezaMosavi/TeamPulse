using System.Diagnostics;
using BuildingBlocks.Application.Abstractions.Behaviors;
using BuildingBlocks.Application.Options;
using BuildingBlocks.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Application.Behaviors;

/// <summary>
/// Represents a CQRS pipeline behavior that measures request execution time and logs
/// requests exceeding the configured performance threshold.
/// </summary>
/// <remarks>
/// This behavior monitors the total execution time of the remaining pipeline and
/// emits a warning log when the elapsed time is greater than the configured threshold.
/// It does not modify the request or its result.
/// </remarks>
public class PerformanceBehavior<TRequest, TResult>(
    ILogger<PerformanceBehavior<TRequest, TResult>> logger,
    IOptions<PerformanceOptions> options)
    : IPipelineBehavior<TRequest, TResult> where TRequest : notnull
{
    private static readonly string _requestName = typeof(TRequest).Name;

    /// <summary>
    /// Measures the execution time of the remaining request pipeline and logs
    /// requests whose execution time exceeds the configured threshold.
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
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var result = await next();

            return result;
        }
        finally
        {
            stopwatch.Stop();
            var elapsedTime = stopwatch.ElapsedMilliseconds;
            var threshold = options.Value.ThresholdMilliseconds;


            if (elapsedTime > threshold)
                logger.LogWarning("Slow request {RequestName} executed in {ElapsedMilliseconds} ms.", _requestName,
                                  elapsedTime);
        }
    }
}