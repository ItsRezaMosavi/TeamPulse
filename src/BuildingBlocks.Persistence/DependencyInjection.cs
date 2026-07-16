using BuildingBlocks.Application.Abstractions.Behaviors;
using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Persistence.Abstractions;
using BuildingBlocks.Persistence.Abstractions.Outbox;
using BuildingBlocks.Persistence.Abstractions.Repositories;
using BuildingBlocks.Persistence.Behaviors;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.Interceptors;
using BuildingBlocks.Persistence.Outbox.Background;
using BuildingBlocks.Persistence.Outbox.Interceptors;
using BuildingBlocks.Persistence.Outbox.Options;
using BuildingBlocks.Persistence.Outbox.Processing;
using BuildingBlocks.Persistence.Outbox.Publishers;
using BuildingBlocks.Persistence.Outbox.Serialization;
using BuildingBlocks.Persistence.Repositories;
using BuildingBlocks.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Persistence;

/// <summary>
/// Provides extension methods for configuring Building Blocks persistence services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
	/// <summary>
	/// Adds Building Blocks persistence services including DbContext, Unit of Work, and repositories to the service collection.
	/// </summary>
	/// <param name="services">The service collection to configure.</param>
	/// <param name="configureDbContext">An action to configure the DbContext options.</param>
	/// <returns>The configured service collection for method chaining.</returns>
	public static IServiceCollection AddBuildingBlocksPersistence<TUserId>(this IServiceCollection services,
																		   Action<IServiceProvider,
																				   DbContextOptionsBuilder>
																			   configureDbContext)
	{
		services.AddDbContext<BuildingBlocksDbContext>((sp, options) =>
		{
			configureDbContext(sp, options);

			options.AddInterceptors(sp.GetServices<IInterceptor>());
		});


		services.AddScoped<IInterceptor, AuditableEntityInterceptor<TUserId>>();
		services.AddScoped<IInterceptor, SoftDeleteInterceptor<TUserId>>();

		services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
		services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));


		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
		return services;
	}


	/// <summary>
	/// Adds the Outbox infrastructure required for reliable integration event publishing.
	/// </summary>
	/// <param name="services">The service collection to configure.</param>
	/// <param name="configureOptions">
	/// An optional action to configure the <see cref="OutboxOptions"/> such as batch size
	/// and polling interval.
	/// </param>
	/// <returns>
	/// The configured <see cref="IServiceCollection"/> for method chaining.
	/// </returns>
	/// <remarks>
	/// This method registers all services required for the Outbox pattern, including:
	/// <list type="bullet">
	/// <item><description>Background service for processing pending outbox messages.</description></item>
	/// <item><description>Integration event publisher for collecting events during the current scope.</description></item>
	/// <item><description>JSON serializer for persisting and restoring integration events.</description></item>
	/// <item><description>EF Core interceptor for storing integration events in the Outbox table.</description></item>
	/// <item><description>Outbox processor responsible for publishing pending messages to the external event bus.</description></item>
	/// </list>
	/// <para>
	/// This method should be called after <see cref="AddBuildingBlocksPersistence{TUserId}"/>
	/// has been configured.
	/// </para>
	/// </remarks>
	public static IServiceCollection AddOutbox(this IServiceCollection services,
											   Action<OutboxOptions>? configureOptions = null)
	{
		services.Configure(configureOptions ?? (_ => { }));
		
		services.AddSingleton<IOutboxSerializer, JsonOutboxSerializer>();

		services.AddScoped<IIntegrationEventPublisher, OutboxIntegrationEventPublisher>();
		services.AddScoped<OutboxProcessor>();
		
		services.AddScoped<IInterceptor, OutboxInterceptor>();
		
		services.AddHostedService<OutboxBackgroundService>();
		
		return services;
	}
}