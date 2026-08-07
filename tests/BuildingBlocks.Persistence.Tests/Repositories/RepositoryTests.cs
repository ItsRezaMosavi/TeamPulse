using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.Repositories;
using BuildingBlocks.Persistence.Specifications;
using BuildingBlocks.Specification.Contracts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Tests.Repositories;

public class RepositoryTests
{
	private readonly EfSpecificationEvaluator _evaluator = new();

	[Fact]
	public async Task AddAsync_should_add_entity_to_context()
	{
		await using var context = CreateContext();

		var repository = new TestRepository(context, _evaluator);
		var entity = new TestAggregate("Reza");

		await repository.AddAsync(entity);

		context.Entry(entity)
			   .State
			   .Should()
			   .Be(EntityState.Added);
	}

	[Fact]
	public async Task AddAsync_should_persist_entity_after_save_changes()
	{
		await using var context = CreateContext();

		var repository = new TestRepository(context, _evaluator);
		var entity = new TestAggregate("Reza");

		await repository.AddAsync(entity);
		await context.SaveChangesAsync();

		var result = await context.Set<TestAggregate>()
								  .SingleOrDefaultAsync(x => x.Id == entity.Id);

		result.Should().NotBeNull();
		result.Name.Should().Be("Reza");
	}

	[Fact]
	public async Task AddRangeAsync_should_add_all_entities_to_context()
	{
		await using var context = CreateContext();

		var repository = new TestRepository(context, _evaluator);

		var entities = new[] { new TestAggregate("Ali"), new TestAggregate("Reza"), new TestAggregate("Jack") };

		await repository.AddRangeAsync(entities);

		entities
			.Select(context.Entry)
			.Select(entry => entry.State)
			.Should()
			.AllSatisfy(state =>
							state.Should().Be(EntityState.Added));
	}

	[Fact]
	public async Task AddRangeAsync_should_persist_all_entities_after_save_changes()
	{
		await using var context = CreateContext();

		var repository = new TestRepository(context, _evaluator);

		var entities = new[] { new TestAggregate("Ali"), new TestAggregate("Reza"), new TestAggregate("Jack") };

		await repository.AddRangeAsync(entities);
		await context.SaveChangesAsync();

		var ids = entities
				  .Select(x => x.Id)
				  .ToList();

		var result = await context.Set<TestAggregate>()
								  .Where(x => ids.Contains(x.Id))
								  .ToListAsync();

		result.Should().HaveCount(3);

		result
			.Select(x => x.Name)
			.Should()
			.BeEquivalentTo("Ali", "Reza", "Jack");
	}

	[Fact]
	public async Task Remove_should_mark_entity_as_deleted()
	{
		await using var context = CreateContext();

		var entity = new TestAggregate("Reza");

		context.Set<TestAggregate>().Add(entity);
		await context.SaveChangesAsync();

		var repository = new TestRepository(context, _evaluator);

		repository.Remove(entity);

		context.Entry(entity)
			   .State
			   .Should()
			   .Be(EntityState.Deleted);
	}

	[Fact]
	public async Task Remove_should_delete_entity_after_save_changes()
	{
		await using var context = CreateContext();

		var entity = new TestAggregate("Reza");

		context.Set<TestAggregate>().Add(entity);
		await context.SaveChangesAsync();

		var repository = new TestRepository(context, _evaluator);

		repository.Remove(entity);
		await context.SaveChangesAsync();

		var result = await context.Set<TestAggregate>()
								  .SingleOrDefaultAsync(x => x.Id == entity.Id);

		result.Should().BeNull();
	}

	[Fact]
	public async Task RemoveRange_should_mark_all_entities_as_deleted()
	{
		await using var context = CreateContext();

		var entities = new[] { new TestAggregate("Ali"), new TestAggregate("Reza"), new TestAggregate("Jack") };

		context.Set<TestAggregate>().AddRange(entities);
		await context.SaveChangesAsync();

		var repository = new TestRepository(context, _evaluator);

		repository.RemoveRange(entities);

		entities
			.Select(context.Entry)
			.Select(entry => entry.State)
			.Should()
			.AllSatisfy(state =>
							state.Should().Be(EntityState.Deleted));
	}

	[Fact]
	public async Task RemoveRange_should_delete_all_entities_after_save_changes()
	{
		await using var context = CreateContext();

		var entities = new[] { new TestAggregate("Ali"), new TestAggregate("Reza"), new TestAggregate("Jack") };

		context.Set<TestAggregate>().AddRange(entities);
		await context.SaveChangesAsync();

		var repository = new TestRepository(context, _evaluator);

		repository.RemoveRange(entities);

		await context.SaveChangesAsync();

		var result = await context.Set<TestAggregate>()
								  .ToListAsync();

		result.Should().BeEmpty();
	}

	private static TestDbContext CreateContext()
	{
		var options = new DbContextOptionsBuilder<BuildingBlocksDbContext>()
					  .UseSqlite("DataSource=:memory:")
					  .Options;

		var context = new TestDbContext(options);

		context.Database.OpenConnection();
		context.Database.EnsureCreated();

		return context;
	}

	private sealed class TestDbContext(
		DbContextOptions<BuildingBlocksDbContext> options)
		: BuildingBlocksDbContext(options)
	{
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<TestAggregate>(builder =>
			{
				builder.HasKey(x => x.Id);

				builder.Property(x => x.Name)
					   .IsRequired();
			});
		}
	}

	private sealed class TestRepository(
		BuildingBlocksDbContext context,
		ISpecificationEvaluator evaluator)
		: Repository<TestAggregate, Guid>(context, evaluator);

	private sealed class TestAggregate(string name) : AggregateRoot
	{
		public string Name { get; private set; } = name;
	}
}