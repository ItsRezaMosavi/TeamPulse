namespace BuildingBlocks.Domain.Entities;

public abstract class AuditableEntity<TId, TUserId> : Entity<TId>
{
    public DateTime CreatedAt { get; protected set; }
    public TUserId? CreatedBy { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public TUserId? UpdatedBy { get; protected set; }

    protected void SetCreated(TUserId? userId)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = userId;
    }

    protected void SetUpdated(TUserId? userId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userId;
    }
}