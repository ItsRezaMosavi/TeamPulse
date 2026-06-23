using BuildingBlocks.Persistence.OutBox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Persistence.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(Settings.TableNames.OutboxMessage, Settings.SchemaNames.BuildingBlock);
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OccurredOnUtc)
               .IsRequired();

        builder.Property(x => x.Type)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(x => x.Content)
               .IsRequired();

        builder.Property(x => x.ProcessedOnUtc);

        builder.Property(x => x.AttemptCount)
               .IsRequired();

        builder.Property(x => x.LastError)
               .HasMaxLength(4000);

        builder.HasIndex(x => x.ProcessedOnUtc);

        builder.HasIndex(x => x.OccurredOnUtc);
    }
}