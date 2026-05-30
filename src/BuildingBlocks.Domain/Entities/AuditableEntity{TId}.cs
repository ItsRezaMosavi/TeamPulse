namespace BuildingBlocks.Domain.Entities;

public abstract class AuditableEntity<TId, TUserId> : Entity<TId>
{
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public TUserId? CreatedBy { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public TUserId? UpdatedBy { get; protected set; }

    public Guid? DeleteCommandCorrelationId { get; protected set; }
    public Guid? InsertCommandCorrelationId { get; protected set; }
    public Guid? UpdateCommandCorrelationId { get; protected set; }
}