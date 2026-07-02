using BuildingBlocks.Application.CQRS.Behaviors;
using BuildingBlocks.Results;
using BuildingBlocks.Results.Errors;
using FluentValidation;
using FluentValidation.Results;

namespace BuildingBlocks.Validation;

/// <summary>
/// Represents a CQRS pipeline behavior that executes all registered
/// <see cref="IValidator{T}"/> instances for the current request before
/// invoking the request handler.
/// </summary>
/// <typeparam name="TRequest">
/// The type of the request being validated.
/// </typeparam>
/// <typeparam name="TResult">
/// The type of the successful result returned by the request handler.
/// </typeparam>
/// <remarks>
/// If one or more validation failures are detected, the request handler
/// is not executed and a failed <see cref="Result{TResult}"/> containing
/// validation errors is returned.
///
/// <para>
/// This behavior acts as the integration point between FluentValidation
/// and the Building Blocks CQRS pipeline.
/// </para>
/// </remarks>
public sealed class ValidationBehavior<TRequest, TResult>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResult>
{
    private readonly IValidator<TRequest>[] _validators = validators.ToArray();

    /// <summary>
    /// Validates the incoming request before executing the next delegate
    /// in the CQRS pipeline.
    /// </summary>
    /// <param name="request">
    /// The request being processed.
    /// </param>
    /// <param name="next">
    /// The next delegate in the pipeline.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the validation operation.
    /// </param>
    /// <returns>
    /// A successful <see cref="Result{TResult}"/> if validation succeeds,
    /// otherwise a failed result containing validation errors.
    /// </returns>
    public async Task<Result<TResult>> HandleAsync(TRequest request,
                                                   RequestHandlerDelegate<TResult> next,
                                                   CancellationToken cancellationToken = default)
    {
        if (_validators.Length == 0)
            return await next();

        var failures = await ValidateAsync(request, cancellationToken);

        if (failures.Count != 0)
            return CreateFailureResult(failures);

        return await next();
    }


    /// <summary>
    /// Executes all registered validators for the specified request.
    /// </summary>
    /// <param name="request">
    /// The request to validate.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the validation operation.
    /// </param>
    /// <returns>
    /// A read-only collection containing all validation failures.
    /// Returns an empty collection if validation succeeds.
    /// </returns>
    private async Task<IReadOnlyCollection<ValidationFailure>> ValidateAsync(TRequest request,
                                                                             CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults =
            await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        return validationResults
              .SelectMany(r => r.Errors)
              .Where(e => e is not null)
              .ToList();
    }

    /// <summary>
    /// Creates a failed <see cref="Result{TResult}"/> from the specified
    /// validation failures.
    /// </summary>
    /// <param name="failures">
    /// The validation failures returned by FluentValidation.
    /// </param>
    /// <returns>
    /// A failed result containing the mapped validation errors.
    /// </returns>
    private static Result<TResult> CreateFailureResult(IReadOnlyCollection<ValidationFailure> failures)
    {
        return failures.DistinctBy(x => new
                        {
                            x.PropertyName,
                            x.ErrorMessage,
                            x.ErrorCode
                        })
                       .Select(f => new ValidationError(f.PropertyName, f.ErrorMessage, f.ErrorCode))
                       .ToArray();
    }
}