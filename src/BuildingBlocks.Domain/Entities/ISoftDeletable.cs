namespace BuildingBlocks.Domain.Entities;

public interface ISoftDeletable<TUserId>
{
    public DateTime? DeletedAt { get; protected set; }
    public TUserId? DeletedBy { get; protected set; }
    bool IsDeleted { get; protected set; }

    virtual void Delete(TUserId userId)
    {
        if (IsDeleted)
            return;
        IsDeleted = true;
        DeletedBy = userId;
        DeletedAt = DateTime.UtcNow;
    }

    virtual void Restore()
    {
        if (!IsDeleted)
            return;
        IsDeleted = false;
        DeletedBy = default;
        DeletedAt = null;
    }
}