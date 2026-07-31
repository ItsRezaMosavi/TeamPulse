namespace BuildingBlocks.Persistence;

/// <summary>
/// Centralized configuration constants for the persistence layer.
/// </summary>
/// <remarks>
/// This class provides consistent naming conventions for database schema objects
/// used throughout the Building Blocks persistence infrastructure. Using these
/// constants ensures uniformity across migrations, queries, and configurations.
/// </remarks>
public static class PersistenceSettings
{
    /// <summary>
    /// Database schema name constants.
    /// </summary>
    /// <remarks>
    /// Schema names organize database objects into logical groups. The default
    /// "BuildingBlock" schema separates building blocks tables from application-specific ones.
    /// </remarks>
    public static class SchemaNames
    {
        /// <summary>
        /// The default schema name for Building Blocks database objects.
        /// </summary>
        public const string BuildingBlock = "BuildingBlock";
    }
    
    
    /// <summary>
    /// Database table name constants.
    /// </summary>
    /// <remarks>
    /// Table names define the physical storage structure for persistence entities.
    /// These constants ensure consistency between entity configurations and raw SQL queries.
    /// </remarks>
    public static class TableNames
    {
        /// <summary>
        /// The table name for storing outbox messages for reliable event delivery.
        /// </summary>
        /// <remarks>
        /// The Outbox pattern stores domain events in the database transaction before
        /// dispatching them externally, ensuring atomicity between state changes and event publication.
        /// </remarks>
        public const string OutboxMessage = "OutboxMessage";
        
        
        public const string IdempotencyRecord =  "IdempotencyRecords";
    }
}