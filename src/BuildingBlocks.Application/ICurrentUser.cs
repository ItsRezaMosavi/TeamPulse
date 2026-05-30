namespace BuildingBlocks.Application;

public interface ICurrentUser<out TId>
{
    bool IsAuthenticated { get; }
    TId? UserId { get; }
    IReadOnlyCollection<string> Roles { get; }
}