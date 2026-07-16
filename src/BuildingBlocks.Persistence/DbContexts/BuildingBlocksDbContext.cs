using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.DbContexts;

/// <summary>
/// The primary Entity Framework Core database context for the Building Blocks infrastructure.
/// </summary>
/// <remarks>
/// This context serves as the central point for data access, managing entity tracking,
/// change detection, and database interactions. It automatically discovers and applies
/// entity configurations from the assembly, enabling fluent API configurations defined
/// in separate configuration classes.
/// 
/// Key features:
/// <list type="bullet">
/// <item><description>Automatic discovery of entity configurations via ApplyConfigurationsFromAssembly</description></item>
/// <item><description>Integration with EF Core interceptors for audit and soft delete functionality</description></item>
/// <item><description>Support for unit of work pattern through SaveChangesAsync</description></item>
/// </list>
/// 
/// Extend this class or create derived contexts to add DbSets for your domain entities.
/// </remarks>
public class BuildingBlocksDbContext(DbContextOptions<BuildingBlocksDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Configures the database schema and entity mappings during context initialization.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    /// <remarks>
    /// This method applies all entity configurations discovered from the current assembly,
    /// allowing separation of entity classes from their database mapping configurations.
    /// Configuration classes implementing IEntityTypeConfiguration should be placed in
    /// the same assembly to be automatically detected.
    /// 
    /// Override this method in derived classes to add custom model configurations or
    /// modify the convention-based model building process.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BuildingBlocksDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}