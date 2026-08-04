using BuildingBlocks.Persistence.Configurations.BaseConfigurations;
using BuildingBlocks.Persistence.Configurations.ConcurrencyEntityConfigurations;
using BuildingBlocks.Persistence.Outbox.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Persistence.Configurations.OutboxConfigurations;

/// <inheritdoc />
public class OutboxMessageConfiguration : EntityBaseConfiguration<OutboxMessage>
{
	/// <inheritdoc />
	public override void Configure(EntityTypeBuilder<OutboxMessage> builder)
	{
		builder.ToTable(PersistenceSettings.TableNames.OutboxMessage, PersistenceSettings.SchemaNames.BuildingBlock);

		builder.ConfigureConcurrency();
	}
}