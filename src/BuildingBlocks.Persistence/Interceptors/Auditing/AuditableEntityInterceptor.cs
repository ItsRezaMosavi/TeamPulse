using BuildingBlocks.Application.Context;
using BuildingBlocks.Domain.Entities.AuditableEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Persistence.Interceptors.Auditing;

/// <summary>
/// Intercepts EF Core save operations to automatically populate audit information on entities.
/// </summary>
/// <typeparam name="TUserId">The type of the user identifier for tracking who created or modified entities.</typeparam>
/// <remarks>
/// This interceptor automatically sets creation and modification audit fields for entities
/// implementing <see cref="IAuditSetter{TUserId}"/>. It runs during <see cref="DbContext.SaveChangesAsync"/>
/// and processes all added or modified entities before persistence.
/// 
/// For added entities:
/// <list type="bullet">
/// <item><description>Sets CreatedAt to current UTC time</description></item>
/// <item><description>Sets CreatedBy to current user ID</description></item>
/// </list>
/// 
/// For modified entities:
/// <list type="bullet">
/// <item><description>Sets UpdatedAt to current UTC time</description></item>
/// <item><description>Sets UpdatedBy to current user ID</description></item>
/// </list>
/// 
/// This ensures consistent audit tracking across all domain entities without requiring
/// manual audit field management in business logic.
/// </remarks>
public class AuditableEntityInterceptor<TUserId>(
    IDateTimeProvider dateTimeProvider,
    ICurrentUser<TUserId> currentUser)
    : SaveChangesInterceptor
{
    /// <summary>
    /// Intercepts the save operation to populate audit fields on added and modified entities.
    /// </summary>
    /// <param name="eventData">Event data containing the DbContext context.</param>
    /// <param name="result">The current interception result.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task containing the interception result.</returns>
    /// <remarks>
    /// The method processes entities based on their state:
    /// <list type="bullet">
    /// <item><description>Added entities: Calls SetCreated with current user ID and timestamp</description></item>
    /// <item><description>Modified entities: Calls SetUpdated with current user ID and timestamp</description></item>
    /// </list>
    /// Entities must implement IAuditSetter to receive audit information. If no context is available,
    /// the operation proceeds without modifications.
    /// </remarks>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
                                                                          InterceptionResult<int> result,
                                                                          CancellationToken cancellationToken = default)
    {
        var now = dateTimeProvider.UtcNow;
        var userId = currentUser.UserId;
        var context = eventData.Context;

        if (context is null)
            return ValueTask.FromResult(result);

        var entries = context.ChangeTracker.Entries()
                             .Where(e => e.State is EntityState.Added or EntityState.Modified &&
                                         e.Entity is IAuditSetter<TUserId>);

        foreach (var entry in entries)
        {
            var entity = (IAuditSetter<TUserId>)entry.Entity;
            switch (entry.State)
            {
                case EntityState.Modified:
                    entity.SetUpdated(userId, now);
                    break;
                case EntityState.Added:
                    entity.SetCreated(userId, now);
                    break;
            }
        }

        return base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }
}