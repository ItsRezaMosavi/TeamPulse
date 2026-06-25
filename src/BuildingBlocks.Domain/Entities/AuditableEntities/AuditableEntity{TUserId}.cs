namespace BuildingBlocks.Domain.Entities.AuditableEntities;


/// <summary>
/// 
/// </summary>
/// <typeparam name="TUserId"></typeparam>
public abstract class AuditableEntity<TUserId> : AuditableEntity<Guid,TUserId>
{
    
}