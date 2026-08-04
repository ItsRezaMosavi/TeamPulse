using BuildingBlocks.Domain.Entities.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Persistence.Configurations.BaseConfigurations;

public abstract class EntityBaseConfiguration<TEntity> : EntityBaseConfiguration<TEntity, Guid>
	where TEntity : Entity<Guid>
{
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.HasKey(e => e.Id);
	}
}