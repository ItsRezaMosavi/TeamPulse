namespace BuildingBlocks.Persistence;

public static class Settings
{
    public static class SchemaNames
    {
        public const string BuildingBlock = "BuildingBlock";
    }
    
    
    public static class TableNames
    {
        public const string OutboxMessage = "OutboxMessage";
    }
}