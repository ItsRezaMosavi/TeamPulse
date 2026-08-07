using BuildingBlocks.Application.Context;
using BuildingBlocks.Domain.Entities;
using BuildingBlocks.Persistence.Interceptors.SoftDelete;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Tests.Interceptors;

public class SoftDeleteInterceptorTests
{
	private static readonly Guid UserId =
		Guid.Parse("11111111-1111-1111-1111-111111111111");

	private static readonly DateTime FixedDate =
		new(2026, 8, 7, 10, 30, 0, DateTimeKind.Utc);

	[Fact]
	public async Task SavingChangesAsync_should_convert_delete_to_soft_delete()
	{
		await using var context = CreateContext();

		var entity = new TestSoftDeletableEntity { Name = "Reza" };

		context.TestEntities.Add(entity);

		await context.SaveChangesAsync();

		context.Remove(entity);

		await context.SaveChangesAsync();

		entity.IsDeleted.Should().BeTrue();
		entity.DeletedAt.Should().Be(FixedDate);
		entity.DeletedBy.Should().Be(UserId);
	}

	[Fact]
	public async Task SavingChangesAsync_should_change_entity_state_from_deleted_to_modified()
	{
		await using var context = CreateContext();

		var entity = new TestSoftDeletableEntity { Name = "Reza" };

		context.TestEntities.Add(entity);

		await context.SaveChangesAsync();

		context.Remove(entity);

		context.Entry(entity)
			   .State
			   .Should()
			   .Be(EntityState.Deleted);

		await context.SaveChangesAsync();

		context.Entry(entity)
			   .State
			   .Should()
			   .Be(EntityState.Unchanged);
	}

	[Fact]
	public async Task SavingChangesAsync_should_not_physically_delete_entity()
	{
		await using var context = CreateContext();

		var entity = new TestSoftDeletableEntity { Name = "Reza" };

		context.TestEntities.Add(entity);

		await context.SaveChangesAsync();

		var id = entity.Id;

		context.Remove(entity);

		await context.SaveChangesAsync();

		var persistedEntity = await context.TestEntities
										   .SingleOrDefaultAsync(x => x.Id == id);

		persistedEntity.Should().NotBeNull();
		persistedEntity!.IsDeleted.Should().BeTrue();
	}

	[Fact]
	public async Task SavingChangesAsync_should_apply_soft_delete_to_multiple_entities()
	{
		await using var context = CreateContext();

		var entities = new[]
		{
			new TestSoftDeletableEntity { Name = "Ali" },
			new TestSoftDeletableEntity { Name = "Reza" },
			new TestSoftDeletableEntity { Name = "Jack" }
		};

		context.TestEntities.AddRange(entities);

		await context.SaveChangesAsync();

		context.RemoveRange(entities);

		await context.SaveChangesAsync();

		entities.Should()
				.AllSatisfy(entity =>
				{
					entity.IsDeleted.Should().BeTrue();
					entity.DeletedAt.Should().Be(FixedDate);
					entity.DeletedBy.Should().Be(UserId);
				});
	}

	[Fact]
	public async Task SavingChangesAsync_should_not_apply_soft_delete_to_non_soft_deletable_entities()
	{
		await using var context = CreateContext();

		var entity = new TestRegularEntity { Name = "Reza" };

		context.RegularEntities.Add(entity);

		await context.SaveChangesAsync();

		context.Remove(entity);

		await context.SaveChangesAsync();

		var result = await context.RegularEntities
								  .SingleOrDefaultAsync(x => x.Id == entity.Id);

		result.Should().BeNull();
	}

	[Fact]
	public async Task SavingChangesAsync_should_not_set_soft_delete_fields_before_entity_is_deleted()
	{
		await using var context = CreateContext();

		var entity = new TestSoftDeletableEntity { Name = "Reza" };

		context.TestEntities.Add(entity);

		await context.SaveChangesAsync();

		entity.IsDeleted.Should().BeFalse();
		entity.DeletedAt.Should().BeNull();
		entity.DeletedBy.Should().BeEmpty();
	}

	private static TestDbContext CreateContext()
	{
		var dateTimeProvider = new TestDateTimeProvider();
		var currentUser = new TestCurrentUser();

		var interceptor = new SoftDeleteInterceptor<Guid>(
														  dateTimeProvider,
														  currentUser);

		var options = new DbContextOptionsBuilder<TestDbContext>()
					  .UseSqlite("DataSource=:memory:")
					  .AddInterceptors(interceptor)
					  .Options;

		var context = new TestDbContext(options);

		context.Database.OpenConnection();
		context.Database.EnsureCreated();

		return context;
	}

	private sealed class TestDbContext(
		DbContextOptions<TestDbContext> options)
		: DbContext(options)
	{
		public DbSet<TestSoftDeletableEntity> TestEntities => Set<TestSoftDeletableEntity>();

		public DbSet<TestRegularEntity> RegularEntities => Set<TestRegularEntity>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TestSoftDeletableEntity>(builder =>
			{
				builder.HasKey(x => x.Id);

				builder.Property(x => x.Name)
					   .IsRequired();
			});

			modelBuilder.Entity<TestRegularEntity>(builder =>
			{
				builder.HasKey(x => x.Id);

				builder.Property(x => x.Name)
					   .IsRequired();
			});
		}
	}

	private sealed class TestSoftDeletableEntity : ISoftDeletable<Guid>
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Name { get; set; } = string.Empty;

		public DateTime? DeletedAt { get; private set; }
		public Guid DeletedBy { get; private set; }
		public bool IsDeleted { get; private set; }

		public void Delete(Guid userId, DateTime deletedAt)
		{
			if (IsDeleted) return;

			IsDeleted = true;
			DeletedAt = deletedAt;
			DeletedBy = userId;
		}

		public void Restore()
		{
			if (!IsDeleted) return;

			IsDeleted = false;
			DeletedBy = Guid.Empty;
			DeletedAt = null;
		}
	}

	private sealed class TestRegularEntity
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Name { get; set; } = string.Empty;
	}

	private sealed class TestDateTimeProvider : IDateTimeProvider
	{
		public DateTime UtcNow => FixedDate;
		public TimeZoneInfo TimeZoneInfo => TimeZoneInfo.Utc;
	}

	private sealed class TestCurrentUser : ICurrentUser<Guid>
	{
		public bool IsAuthenticated { get; }
		public Guid UserId => SoftDeleteInterceptorTests.UserId;
		public string? UserName { get; }
		public IReadOnlyCollection<string> Roles { get; }
		public IReadOnlyCollection<UserClaim> Claims { get; }
	}
}