using BuildingBlocks.Results;

namespace BuildingBlocks.Application.CQRS.Behaviors;

public interface IPipelineBehavior<in TRequest, TResult>
{
    Task<Result<TResult>> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken = default);
}