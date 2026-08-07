using BuildingBlocks.Domain.Entities.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Persistence.Configurations.BaseConfigurations;

public abstract class EntityBaseConfiguration<TEntity> : EntityBaseConfiguration<TEntity, Guid>
	where TEntity : Entity<Guid>
{
	public override void Configure(EntityTypeBuilder<TEntity> builder)
	{
		base.Configure(builder);

		builder.Property(x => x.Id)
			   .ValueGeneratedNever();
	}
}