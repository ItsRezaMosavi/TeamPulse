using BuildingBlocks.Application.Abstractions.Behaviors;
using BuildingBlocks.Application.Behaviors;
using BuildingBlocks.Application.Options;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddBuildingBlocksApplication(this IServiceCollection services,
																  Action<PerformanceOptions>? configure = null)
	{
		services.AddOptions<PerformanceOptions>();

		if (configure is not null) services.Configure(configure);

		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

		return services;
	}
}