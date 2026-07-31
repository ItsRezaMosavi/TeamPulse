using BuildingBlocks.Persistence.Outbox.Entities;
using BuildingBlocks.Persistence.Outbox.Enums;
using BuildingBlocks.Specification.Base;

namespace BuildingBlocks.Persistence.Outbox.QuerySpecifications;

public class OutboxMessageQuery : QuerySpecification<OutboxMessage>
{
	public OutboxMessageQuery(DateTime now,
										   TimeSpan processingTimeout,
										   int batchSize,
										   TimeSpan retryDelay)
	{
		var processingExpiredAt = now.Subtract(processingTimeout);
		var retryAvailableAt = now.Subtract(retryDelay);

		Criteria = message =>
			(
				message.Status == OutboxStatus.Pending &&
				(
					message.LastAttemptOnUtc == null ||
					message.LastAttemptOnUtc <= retryAvailableAt
				)
			)
			||
			(
				message.Status == OutboxStatus.Processing &&
				message.ProcessingStartedOnUtc != null &&
				message.ProcessingStartedOnUtc <= processingExpiredAt
			);


		ApplyOrderBy(message => message.OccurredOnUtc);

		ApplyPaging(0, batchSize);
	}
}