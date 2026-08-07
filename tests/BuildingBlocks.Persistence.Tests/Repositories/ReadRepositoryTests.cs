using System.Linq.Expressions;
using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.Repositories;
using BuildingBlocks.Persistence.Specifications;
using BuildingBlocks.Specification.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Tests.Repositories;

public class ReadRepositoryTests
{
	private readonly EfSpecificationEvaluator _evaluator = new();

	[Fact]
	public async Task GetByIdAsync_should_return_entity_when_id_exists()
	{
		await using var context = CreateContext();

		var entity = new TestAggregate(Guid.NewGuid(), "Reza");

		context.Set<TestAggregate>().Add(entity);
		await context.SaveChangesAsync();

		var repository = new TestReadRepository(context, _evaluator);

		var result = await repository.GetByIdAsync(entity.Id);

		result.Should().NotBeNull();
		result.Id.Should().Be(entity.Id);
		result.Name.Should().Be("Reza");
	}

	[Fact]
	public async Task GetByIdAsync_should_return_null_when_id_does_not_exist()
	{
		await using var context = CreateContext();

		var repository = new TestReadRepository(context, _evaluator);

		var result = await repository.GetByIdAsync(Guid.NewGuid());

		result.Should().BeNull();
	}

	[Fact]
	public async Task ListAsync_should_return_all_entities_when_specification_is_null()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var result = await repository.ListAsync();

		result.Should().HaveCount(3);

		result
			.Select(x => x.Name)
			.Should()
			.BeEquivalentTo("Ali", "Reza", "Jack");
	}

	[Fact]
	public async Task ListAsync_should_apply_specification()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Name == "Reza");

		var result = await repository.ListAsync(specification);

		result.Should().ContainSingle();
		result.Single().Name.Should().Be("Reza");
	}

	[Fact]
	public async Task ListAsync_should_apply_projection()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestProjectionSpecification();

		specification.SetSelector(x => new TestResult { Id = x.Id, Name = x.Name });

		var result = await repository.ListAsync(specification);

		result.Should().HaveCount(3);

		result
			.Select(x => x.Name)
			.Should()
			.BeEquivalentTo("Ali", "Reza", "Jack");

		result
			.Select(x => x.Id)
			.Should()
			.AllSatisfy(id => id.Should().NotBeEmpty());
	}

	[Fact]
	public async Task SingleOrDefaultAsync_should_return_entity_when_specification_matches_one_entity()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Name == "Reza");

		var result = await repository.SingleOrDefaultAsync(specification);

		result.Should().NotBeNull();
		result!.Name.Should().Be("Reza");
	}

	[Fact]
	public async Task SingleOrDefaultAsync_should_return_null_when_specification_matches_no_entity()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Name == "DoesNotExist");

		var result = await repository.SingleOrDefaultAsync(specification);

		result.Should().BeNull();
	}

	[Fact]
	public async Task SingleOrDefaultAsync_should_throw_when_specification_matches_multiple_entities()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Name.Contains("a"));

		var action = () => repository.SingleOrDefaultAsync(specification);

		await action
			  .Should()
			  .ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public async Task SingleAsync_should_return_entity_when_specification_matches_one_entity()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Name == "Reza");

		var result = await repository.SingleAsync(specification);

		result.Should().NotBeNull();
		result.Name.Should().Be("Reza");
	}

	[Fact]
	public async Task SingleAsync_should_throw_when_specification_matches_no_entity()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Name == "DoesNotExist");

		var action = () => repository.SingleAsync(specification);

		await action
			  .Should()
			  .ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public async Task SingleAsync_should_throw_when_specification_matches_multiple_entities()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Name.Contains("a"));

		var action = () => repository.SingleAsync(specification);

		await action
			  .Should()
			  .ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public async Task FirstOrDefaultAsync_should_return_first_matching_entity()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetOrderBy(x => x.Name);

		var result = await repository.FirstOrDefaultAsync(specification);

		result.Should().NotBeNull();
		result!.Name.Should().Be("Ali");
	}

	[Fact]
	public async Task FirstOrDefaultAsync_should_return_null_when_no_entity_matches()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Name == "DoesNotExist");

		var result = await repository.FirstOrDefaultAsync(specification);

		result.Should().BeNull();
	}

	[Fact]
	public async Task FirstAsync_should_return_first_matching_entity()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetOrderBy(x => x.Name);

		var result = await repository.FirstAsync(specification);

		result.Should().NotBeNull();
		result.Name.Should().Be("Ali");
	}

	[Fact]
	public async Task FirstAsync_should_throw_when_no_entity_matches()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Name == "DoesNotExist");

		var action = () => repository.FirstAsync(specification);

		await action
			  .Should()
			  .ThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public async Task AnyAsync_should_return_true_when_specification_matches()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Name == "Reza");

		var result = await repository.AnyAsync(specification);

		result.Should().BeTrue();
	}

	[Fact]
	public async Task AnyAsync_should_return_false_when_specification_does_not_match()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Name == "DoesNotExist");

		var result = await repository.AnyAsync(specification);

		result.Should().BeFalse();
	}

	[Fact]
	public async Task ExistsAsync_should_return_true_when_id_exists()
	{
		await using var context = CreateContext();

		var entity = new TestAggregate(Guid.NewGuid(), "Reza");

		context.Set<TestAggregate>().Add(entity);
		await context.SaveChangesAsync();

		var repository = new TestReadRepository(context, _evaluator);

		var result = await repository.ExistsAsync(entity.Id);

		result.Should().BeTrue();
	}

	[Fact]
	public async Task ExistsAsync_should_return_false_when_id_does_not_exist()
	{
		await using var context = CreateContext();

		var repository = new TestReadRepository(context, _evaluator);

		var result = await repository.ExistsAsync(Guid.NewGuid());

		result.Should().BeFalse();
	}

	[Fact]
	public async Task CountAsync_should_return_total_count_when_specification_is_null()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var result = await repository.CountAsync();

		result.Should().Be(3);
	}

	[Fact]
	public async Task CountAsync_should_return_filtered_count_when_specification_is_provided()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var repository = new TestReadRepository(context, _evaluator);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Name.Contains("e"));

		var result = await repository.CountAsync(specification);

		result.Should().Be(1);
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

	private static async Task SeedAsync(TestDbContext context)
	{
		context.Set<TestAggregate>()
			   .AddRange(
						 new TestAggregate(Guid.NewGuid(), "Ali"),
						 new TestAggregate(Guid.NewGuid(), "Reza"),
						 new TestAggregate(Guid.NewGuid(), "Jack"));

		await context.SaveChangesAsync();
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

	private sealed class TestReadRepository(
		BuildingBlocksDbContext context,
		EfSpecificationEvaluator evaluator)
		: ReadRepository<TestAggregate>(context, evaluator);

	private sealed class TestAggregate(
		Guid id,
		string name)
		: AggregateRoot
	{
		public string Name { get; private set; } = name;
	}

	private sealed class TestResult
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = string.Empty;
	}

	private sealed class TestQuerySpecification
		: QuerySpecification<TestAggregate>
	{
		public void SetCriteria(Expression<Func<TestAggregate, bool>> criteria)
		{
			Criteria = criteria;
		}

		public void SetOrderBy(Expression<Func<TestAggregate, object>> orderBy,
							   bool isAscending = true)
		{
			ApplyOrderBy(orderBy, isAscending);
		}
	}

	private sealed class TestProjectionSpecification
		: QuerySpecification<TestAggregate, TestResult>
	{
		public void SetCriteria(Expression<Func<TestAggregate, bool>> criteria)
		{
			Criteria = criteria;
		}

		public void SetOrderBy(Expression<Func<TestAggregate, object>> orderBy,
							   bool isAscending = true)
		{
			ApplyOrderBy(orderBy, isAscending);
		}

		public void SetSelector(Expression<Func<TestAggregate, TestResult>> selector)
		{
			ApplySelector(selector);
		}
	}
}