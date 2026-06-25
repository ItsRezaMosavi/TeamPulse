using BuildingBlocks.Application.Context;
using BuildingBlocks.Domain.Entities.AuditableEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Persistence.Interceptors;

public class AuditableEntityInterceptor<TUserId>(
    IDateTimeProvider dateTimeProvider,
    ICurrentUser<TUserId> currentUser)
    : SaveChangesInterceptor
{
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