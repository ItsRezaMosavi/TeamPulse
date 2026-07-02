using System.Diagnostics;
using BuildingBlocks.Application.Abstractions.Behaviors;
using BuildingBlocks.Application.Options;
using BuildingBlocks.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Application.Behaviors;

public class PerformanceBehavior<TRequest, TResult>(
    ILogger<PerformanceBehavior<TRequest, TResult>> logger,
    IOptions<PerformanceOptions> options)
    : IPipelineBehavior<TRequest, TResult> where TRequest : notnull
{
    private static readonly string _requestName = typeof(TRequest).Name;

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