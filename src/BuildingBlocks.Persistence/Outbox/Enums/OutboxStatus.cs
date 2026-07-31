namespace BuildingBlocks.Persistence.Outbox.Enums;

public enum OutboxStatus
{
	Pending = 0,
	Processing = 1,
	Completed = 2,
	Failed = 3
}