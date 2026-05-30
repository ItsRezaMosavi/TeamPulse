namespace BuildingBlocks.Domain.Entities;

public interface ISoftDeletable<TUserId>
{
    
    public DateTime? DeletedAt { get; protected set; }
    public TUserId? DeletedBy { get; protected set; }
    bool IsDeleted { get; protected set; }

    virtual void Delete()
    {
        if (IsDeleted)
            return;
        IsDeleted = true;
    }
}