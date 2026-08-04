using BuildingBlocks.Domain.Entities;
using BuildingBlocks.Domain.Entities.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Persistence.Configurations.SoftDeletableConfigurations;

public static class SoftDeletableConfiguration
{
	public static void ConfigureSoftDeletable<TEntity, TId, TUserId>(this EntityTypeBuilder<TEntity> builder)
		where TEntity : Entity<TId>, ISoftDeletable<TUserId>
	{
		builder.Property(x => x.DeletedAt);
		builder.Property(x => x.DeletedBy);
		builder.Property(x => x.IsDeleted).IsRequired();
	}

	public static void ConfigureSoftDeletable<TEntity, TId>(this EntityTypeBuilder<TEntity> builder)
		where TEntity : Entity<TId>, ISoftDeletable
	{
		builder.Property(x => x.DeletedAt);
		builder.Property(x => x.DeletedBy);
		builder.Property(x => x.IsDeleted).IsRequired();
	}

	public static void ConfigureSoftDeletable<TEntity>(this EntityTypeBuilder<TEntity> builder)
		where TEntity : Entity, ISoftDeletable
	{
		builder.Property(x => x.DeletedAt);
		builder.Property(x => x.DeletedBy);
		builder.Property(x => x.IsDeleted).IsRequired();
	}
}