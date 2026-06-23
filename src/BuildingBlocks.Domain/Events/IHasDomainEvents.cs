namespace BuildingBlocks.Domain.Events;

public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; } 
    void ClearDomainEvents();
}