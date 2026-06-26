using BuildingBlocks.Application.CQRS.Behaviors;
using BuildingBlocks.Results;
using BuildingBlocks.Results.Errors;
using FluentValidation;
using FluentValidation.Results;

namespace BuildingBlocks.Validation;

public sealed class ValidationBehavior<TRequest, TResult>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResult>
{
    public async Task<Result<TResult>> HandleAsync(TRequest request,
                                                   RequestHandlerDelegate<TResult> next,
                                                   CancellationToken cancellationToken = default)
    {
        if (!validators.Any())
            return await next();

        var failures = await ValidateAsync(request, cancellationToken);

        if (failures.Any())
            return CreateFailureResult(failures);

        return await next();
    }

    private async Task<IReadOnlyList<ValidationFailure>> ValidateAsync(TRequest request,
                                                                       CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        return validationResults
              .Where(r => !r.IsValid)
              .SelectMany(r => r.Errors)
              .ToList();
    }


    private Result<TResult> CreateFailureResult(IReadOnlyList<ValidationFailure> failures)
    {
        return failures.DistinctBy(f => new { f.PropertyName, f.ErrorCode, f.ErrorMessage })
                       .Select(f => new ValidationError(f.ErrorMessage, f.PropertyName, f.ErrorCode))
                       .ToArray();
    }
}