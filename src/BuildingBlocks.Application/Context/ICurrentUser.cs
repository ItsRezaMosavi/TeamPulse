namespace BuildingBlocks.Application.Context;

/// <summary>
    /// Represents the currently authenticated user with access to user identity, roles, and claims.
    /// </summary>
/// <typeparam name="TUserId"></typeparam>
public interface ICurrentUser<out TUserId>
{
    bool IsAuthenticated { get; }
    TUserId? UserId { get; }
    string? UserName { get; }
    IReadOnlyCollection<string> Roles { get; }
    bool IsInRole(string role) => Roles.Contains(role);

    IReadOnlyCollection<UserClaim> Claims { get; }

    bool HasClaim(string claimType) => Claims.Any(c => c.Type == claimType);
    bool HasClaim(string claimType, string claimValue) => Claims.Any(c => c.Type == claimType && c.Value == claimValue);
}