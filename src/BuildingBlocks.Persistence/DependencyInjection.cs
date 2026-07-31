using BuildingBlocks.Application.Abstractions.Behaviors;
using BuildingBlocks.Application.Abstractions.Idempotency;
using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Persistence.Abstractions;
using BuildingBlocks.Persistence.Abstractions.Outbox;
using BuildingBlocks.Persistence.Abstractions.Repositories;
using BuildingBlocks.Persistence.Behaviors;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.Idempotency;
using BuildingBlocks.Persistence.Interceptors.Auditing;
using BuildingBlocks.Persistence.Interceptors.Outbox;
using BuildingBlocks.Persistence.Interceptors.SoftDelete;
using BuildingBlocks.Persistence.Outbox.Background;
using BuildingBlocks.Persistence.Outbox.Collectors;
using BuildingBlocks.Persistence.Outbox.Options;
using BuildingBlocks.Persistence.Outbox.Processors;
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
	/// <item><description>Integration event collector for collecting events during the current scope.</description></item>
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

		services.AddScoped<IOutboxEventCollector, OutboxEventCollector>();
		services.AddScoped<OutboxProcessor>();

		services.AddScoped<IInterceptor, OutboxInterceptor>();

		services.AddHostedService<OutboxBackgroundService>();

		services.AddSingleton<OutboxWorkerIdentity>();

		return services;
	}

	/// <summary>
	/// Adds the infrastructure required to support idempotent command processing.
	/// </summary>
	/// <param name="services">The service collection to configure.</param>
	/// <param name="configure">
	/// An action used to configure <see cref="IdempotencyOptions"/>, such as the
	/// expiration duration and whether failed responses should be cached.
	/// </param>
	/// <returns>
	/// The configured <see cref="IServiceCollection"/> for method chaining.
	/// </returns>
	/// <remarks>
	/// This method registers all services required for the Idempotency pattern,
	/// including:
	/// <list type="bullet">
	/// <item>
	/// <description>
	/// An <see cref="IIdempotencyStore"/> implementation responsible for acquiring,
	/// completing, releasing, and cleaning up idempotency records.
	/// </description>
	/// </item>
	/// <item>
	/// <description>
	/// An <see cref="IIdempotencySerializer"/> implementation used to serialize and
	/// deserialize cached command responses.
	/// </description>
	/// </item>
	/// <item>
	/// <description>
	/// Configuration for <see cref="IdempotencyOptions"/>, including expiration time
	/// and failure caching behavior.
	/// </description>
	/// </item>
	/// </list>
	/// <para>
	/// This method only registers the required infrastructure. To enable idempotent
	/// command execution, the corresponding <c>IdempotencyBehavior&lt;,&gt;</c>
	/// pipeline behavior must also be registered in the application layer.
	/// </para>
	/// <para>
	/// This method should typically be called after
	/// <see cref="AddBuildingBlocksPersistence{TUserId}(IServiceCollection, Action{IServiceProvider, DbContextOptionsBuilder})"/>
	/// has been configured.
	/// </para>
	/// </remarks>
	public static IServiceCollection AddIdempotency(this IServiceCollection services,
													Action<IdempotencyOptions>? configure = null)
	{
		services.Configure(configure ?? (_ => {}));

		services.AddScoped<IIdempotencyStore, EfCoreIdempotencyStore>();
		services.AddScoped<IIdempotencySerializer, JsonIdempotencySerializer>();

		return services;
	}
}