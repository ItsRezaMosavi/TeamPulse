using BuildingBlocks.Persistence.Outbox.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Persistence.Configurations.OutboxConfigurations;

/// <inheritdoc />
public class OutboxMessageConfiguration : EntityBaseConfiguration<OutboxMessage, Guid>
{
	/// <inheritdoc />
	public override void Configure(EntityTypeBuilder<OutboxMessage> builder)
	{
		builder.ToTable(PersistenceSettings.TableNames.OutboxMessage, PersistenceSettings.SchemaNames.BuildingBlock);

		builder.Property(x => x.RowVersion).IsRowVersion();
	}
}