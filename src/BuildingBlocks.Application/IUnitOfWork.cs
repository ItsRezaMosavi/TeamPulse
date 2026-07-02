namespace BuildingBlocks.Application;

/// <summary>
/// Represents the Unit of Work pattern for managing database transactions and coordinating repositories.
/// </summary>
/// <remarks>
/// The Unit of Work pattern maintains a list of objects affected by a business transaction
/// and coordinates the writing out of changes and the resolution of concurrency problems.
/// 
/// This interface provides:
/// <list type="bullet">
/// <item><description>Transaction management across multiple repository operations</description></item>
/// <item><description>Atomic commit or rollback of all changes</description></item>
/// <item><description>Async disposal support for proper resource cleanup</description></item>
/// </list>
/// 
/// Usage: Wrap related repository operations in a unit of work to ensure they succeed
/// or fail together as a single atomic operation.
/// </remarks>
public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    /// Saves all pending changes tracked by the unit of work to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The number of state entries written to the database.</returns>
    /// <remarks>
    /// This method commits all changes made through repositories participating in this
    /// unit of work. If any part of the save fails, the entire transaction is rolled back.
    /// 
    /// Call this method after completing all repository operations within a transaction scope.
    /// </remarks>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}