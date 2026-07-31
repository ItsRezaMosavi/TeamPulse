using BuildingBlocks.Persistence.Idempotency;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Persistence.Configurations.IdempotencyConfigurations;

/// <inheritdoc />
public class IdempotencyConfiguration : EntityBaseConfiguration<IdempotencyRecord, Guid>
{
	/// <inheritdoc />
	public override void Configure(EntityTypeBuilder<IdempotencyRecord> builder)
	{
		builder.ToTable(PersistenceSettings.TableNames.IdempotencyRecord,
						PersistenceSettings.SchemaNames.BuildingBlock);

		builder.Property(x => x.Key)
			   .IsRequired()
			   .HasMaxLength(200);

		builder.HasIndex(x => x.Key)
			   .IsUnique();

		builder.Property(x => x.Status)
			   .IsRequired();

		builder.Property(x => x.SerializedResponse);

		builder.Property(x => x.CreatedAtUtc)
			   .IsRequired();

		builder.Property(x => x.ExpiresAtUtc)
			   .IsRequired();

		builder.Property(x => x.RowVersion)
			   .IsRowVersion();
	}
}