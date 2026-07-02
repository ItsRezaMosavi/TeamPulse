using BuildingBlocks.Application.CQRS.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Validation;

/// <summary>
/// Provides extension methods for registering the validation infrastructure.
/// </summary>
/// <remarks>
/// This class registers the components required to integrate
/// <see href="https://docs.fluentvalidation.net/">FluentValidation</see>
/// with the Building Blocks CQRS pipeline.
///
/// <para>
/// This method only registers the validation pipeline behavior.
/// Validators should be registered separately using FluentValidation's
/// registration extensions such as
/// <c>AddValidatorsFromAssemblyContaining&lt;T&gt;()</c>.
/// </para>
/// </remarks>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the Building Blocks validation infrastructure.
    /// </summary>
    /// <param name="services">
    /// The service collection used to register application services.
    /// </param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance so that
    /// additional service registrations can be chained.
    /// </returns>
    public static IServiceCollection AddBuildingBlocksValidation(this IServiceCollection services)
    {
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}