using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Persistence.Abstractions.Outbox;
using BuildingBlocks.Persistence.Outbox.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Persistence.Outbox.Interceptors;

public class OutboxInterceptor(IIntegrationEventPublisher publisher, IOutboxSerializer serializer)
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is not null)
            AddOutboxMessages(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
            AddOutboxMessages(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    private void AddOutboxMessages(DbContext context)
    {
        var integrationEvents = publisher.GetEvents();

        if (integrationEvents.Count == 0)
            return;

        var serializedEvents = integrationEvents.Select(serializer.Serialize).ToList();

        var outboxMessages = serializedEvents.Select(OutboxMessage.Create).ToList();

        context.Set<OutboxMessage>().AddRange(outboxMessages);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        publisher.Clear();

        return base.SavedChanges(eventData, result);
    }

    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        publisher.Clear();

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}