namespace BuildingBlocks.Domain.Entities.AuditableEntities;

public interface IAuditSetter<in TUserId>
{
    void SetCreated(TUserId? userId, DateTime createdAt);
    void SetUpdated(TUserId? userId, DateTime updatedAt);
}