using BuildingBlocks.Domain.Entities;
using BuildingBlocks.Domain.Entities.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Persistence.Configurations.ConcurrencyEntityConfigurations;

public static class ConcurrencyEntityConfiguration
{
	public static void ConfigureConcurrency<TEntity, TId>(this EntityTypeBuilder<TEntity> builder)
		where TEntity : Entity<TId>, IConcurrencyEntity
	{
		builder.Property(x => x.RowVersion).IsRowVersion();
	}

	public static void ConfigureConcurrency<TEntity>(this EntityTypeBuilder<TEntity> builder)
		where TEntity : Entity<Guid>, IConcurrencyEntity
	{
		builder.Property(x => x.RowVersion).IsRowVersion();
	}
}