using BuildingBlocks.Application.Context;
using BuildingBlocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Persistence.Interceptors;

public class SoftDeleteInterceptor<TUserId>(IDateTimeProvider dateTimeProvider, ICurrentUser<TUserId> currentUser)
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
                             .Where(e => e.State is EntityState.Deleted &&
                                         e.Entity is ISoftDeletable<TUserId>);

        foreach (var entry in entries)
        {
            var entity = (ISoftDeletable<TUserId>)entry.Entity;
            entity.Delete(userId, now);
            entry.State = EntityState.Modified;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}