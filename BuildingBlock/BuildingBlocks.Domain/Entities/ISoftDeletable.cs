namespace BuildingBlocks.Domain.Entities;

public interface ISoftDeletable
{
    bool IsDeleted { get; protected set; }

    virtual void Delete()
    {
        if (IsDeleted)
            return;
        IsDeleted = true;
    }
}