using BuildingBlocks.Domain.Entities.AuditableEntities;
using BuildingBlocks.Domain.Entities.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Persistence.Configurations.AuditableEntityConfigurations;

public static class AuditableEntityConfiguration
{
	public static void ConfigureAuditable<TEntity, TId, TUserId>(EntityTypeBuilder<TEntity> builder)
		where TEntity : Entity<TId>, IAuditableEntity<TUserId>
	{
		builder.Property(x => x.CreatedAt).IsRequired();
		builder.Property(x => x.CreatedBy);
		builder.Property(x => x.UpdatedAt);
		builder.Property(x => x.UpdatedBy);
	}

	public static void ConfigureAuditable<TEntity, TUserId>(EntityTypeBuilder<TEntity> builder)
		where TEntity : Entity, IAuditableEntity<TUserId>
	{
		builder.Property(x => x.CreatedAt).IsRequired();
		builder.Property(x => x.CreatedBy);
		builder.Property(x => x.UpdatedAt);
		builder.Property(x => x.UpdatedBy);
	}

	public static void ConfigureAuditable<TEntity>(EntityTypeBuilder<TEntity> builder)
		where TEntity : Entity, IAuditableEntity
	{
		builder.Property(x => x.CreatedAt).IsRequired();
		builder.Property(x => x.CreatedBy);
		builder.Property(x => x.UpdatedAt);
		builder.Property(x => x.UpdatedBy);
	}
}