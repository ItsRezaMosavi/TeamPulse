using BuildingBlocks.Application.Context;
using BuildingBlocks.Domain.Entities.AuditableEntities;
using BuildingBlocks.Persistence.Interceptors.Auditing;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Tests.Interceptors;

public class AuditableEntityInterceptorTests
{
	private static readonly Guid UserId =
		Guid.Parse("11111111-1111-1111-1111-111111111111");

	private static readonly DateTime FixedDate =
		new(2026, 8, 7, 10, 30, 0, DateTimeKind.Utc);

	[Fact]
	public async Task SavingChangesAsync_should_set_created_audit_fields_for_added_entity()
	{
		await using var context = CreateContext();

		var entity = new TestAuditableEntity { Name = "Reza" };

		context.TestEntities.Add(entity);

		await context.SaveChangesAsync();

		entity.CreatedAt.Should().Be(FixedDate);
		entity.CreatedBy.Should().Be(UserId);
	}

	[Fact]
	public async Task SavingChangesAsync_should_set_updated_audit_fields_for_modified_entity()
	{
		await using var context = CreateContext();

		var entity = new TestAuditableEntity { Name = "Reza" };

		context.TestEntities.Add(entity);

		await context.SaveChangesAsync();

		entity.UpdatedAt.Should().BeNull();
		entity.UpdatedBy.Should().BeNull();

		entity.Name = "Reza Updated";

		await context.SaveChangesAsync();

		entity.UpdatedAt.Should().Be(FixedDate);
		entity.UpdatedBy.Should().Be(UserId);
	}

	[Fact]
	public async Task SavingChangesAsync_should_not_set_updated_fields_for_added_entity()
	{
		await using var context = CreateContext();

		var entity = new TestAuditableEntity { Name = "Reza" };

		context.TestEntities.Add(entity);

		await context.SaveChangesAsync();

		entity.UpdatedAt.Should().BeNull();
		entity.UpdatedBy.Should().BeNull();
	}

	[Fact]
	public async Task SavingChangesAsync_should_not_change_created_fields_when_entity_is_modified()
	{
		await using var context = CreateContext();

		var entity = new TestAuditableEntity { Name = "Reza" };

		context.TestEntities.Add(entity);

		await context.SaveChangesAsync();

		var originalCreatedAt = entity.CreatedAt;
		var originalCreatedBy = entity.CreatedBy;

		entity.Name = "Reza Updated";

		await context.SaveChangesAsync();

		entity.CreatedAt.Should().Be(originalCreatedAt);
		entity.CreatedBy.Should().Be(originalCreatedBy);

		entity.UpdatedAt.Should().Be(FixedDate);
		entity.UpdatedBy.Should().Be(UserId);
	}

	[Fact]
	public async Task SavingChangesAsync_should_apply_audit_fields_to_multiple_entities()
	{
		await using var context = CreateContext();

		var entities = new[]
		{
			new TestAuditableEntity { Name = "Ali" },
			new TestAuditableEntity { Name = "Reza" },
			new TestAuditableEntity { Name = "Jack" }
		};

		context.TestEntities.AddRange(entities);

		await context.SaveChangesAsync();

		entities.Should()
				.AllSatisfy(entity =>
				{
					entity.CreatedAt.Should().Be(FixedDate);
					entity.CreatedBy.Should().Be(UserId);
					entity.UpdatedAt.Should().BeNull();
					entity.UpdatedBy.Should().BeNull();
				});
	}

	[Fact]
	public async Task SavingChangesAsync_should_not_apply_audit_fields_to_non_auditable_entities()
	{
		await using var context = CreateContext();

		var entity = new TestNonAuditableEntity { Name = "Reza" };

		context.NonAuditableEntities.Add(entity);

		await context.SaveChangesAsync();

		entity.Name.Should().Be("Reza");
	}

	private static TestDbContext CreateContext()
	{
		var dateTimeProvider = new TestDateTimeProvider();
		var currentUser = new TestCurrentUser();

		var interceptor = new AuditableEntityInterceptor<Guid>(
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
		public DbSet<TestAuditableEntity> TestEntities => Set<TestAuditableEntity>();

		public DbSet<TestNonAuditableEntity> NonAuditableEntities => Set<TestNonAuditableEntity>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TestAuditableEntity>(builder =>
			{
				builder.HasKey(x => x.Id);

				builder.Property(x => x.Name)
					   .IsRequired();
			});

			modelBuilder.Entity<TestNonAuditableEntity>(builder =>
			{
				builder.HasKey(x => x.Id);

				builder.Property(x => x.Name)
					   .IsRequired();
			});
		}
	}

	private sealed class TestAuditableEntity : IAuditSetter<Guid>
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Name { get; set; } = string.Empty;

		public DateTime CreatedAt { get; private set; }

		public Guid CreatedBy { get; private set; }

		public DateTime? UpdatedAt { get; private set; }

		public Guid? UpdatedBy { get; private set; }

		public void SetCreated(Guid userId, DateTime createdAt)
		{
			CreatedBy = userId;
			CreatedAt = createdAt;
		}

		public void SetUpdated(Guid userId, DateTime updatedAt)
		{
			UpdatedBy = userId;
			UpdatedAt = updatedAt;
		}
	}

	private sealed class TestNonAuditableEntity
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
		public bool IsAuthenticated => true;

		public Guid UserId => AuditableEntityInterceptorTests.UserId;

		public string? UserName => "TestUser";

		public IReadOnlyCollection<string> Roles => [];

		public IReadOnlyCollection<UserClaim> Claims => [];
	}
}