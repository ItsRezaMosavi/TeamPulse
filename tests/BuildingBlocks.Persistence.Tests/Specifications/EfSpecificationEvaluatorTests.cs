using System.Linq.Expressions;
using BuildingBlocks.Persistence.Specifications;
using BuildingBlocks.Specification.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Tests.Specifications;

public class EfSpecificationEvaluatorTests
{
	private readonly EfSpecificationEvaluator _evaluator = new();

	[Fact]
	public async Task GetQuery_should_apply_criteria()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var specification = new TestQuerySpecification();
		specification.SetCriteria(x => x.Id > 1);

		var result = _evaluator
					 .GetQuery(context.TestEntities, specification)
					 .ToList();

		result.Should().HaveCount(2);
		result.Should().OnlyContain(x => x.Id > 1);
	}

	[Fact]
	public async Task GetQuery_should_apply_includes()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var specification = new TestQuerySpecification();
		specification.AddInclude(x => x.Item);

		var result = _evaluator
					 .GetQuery(context.TestEntities, specification)
					 .ToList();

		result.Should().HaveCount(3);
		result.Should().OnlyContain(x => x.Item != null);

		result
			.Select(x => x.Item.Id)
			.Should()
			.BeEquivalentTo([1, 2, 3]);
	}

	[Fact]
	public async Task GetQuery_should_apply_ascending_order()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var specification = new TestQuerySpecification();
		specification.SetOrderBy(x => x.Id);

		var result = _evaluator
					 .GetQuery(context.TestEntities, specification)
					 .ToList();

		result.Should().HaveCount(3);
		result.Should().BeInAscendingOrder(x => x.Id);
	}

	[Fact]
	public async Task GetQuery_should_apply_descending_order()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var specification = new TestQuerySpecification();
		specification.SetOrderBy(x => x.Id, false);

		var result = _evaluator
					 .GetQuery(context.TestEntities, specification)
					 .ToList();

		result.Should().HaveCount(3);
		result.Should().BeInDescendingOrder(x => x.Id);
	}

	[Fact]
	public async Task GetQuery_should_apply_paging()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var specification = new TestQuerySpecification();

		specification.SetOrderBy(x => x.Id);
		specification.SetPaging(1, 1);

		var result = _evaluator
					 .GetQuery(context.TestEntities, specification)
					 .ToList();

		result.Should().ContainSingle();
		result.Single().Id.Should().Be(2);
	}

	[Fact]
	public async Task GetQuery_should_apply_no_tracking_by_default()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var specification = new TestQuerySpecification();

		var result = _evaluator
					 .GetQuery(context.TestEntities, specification)
					 .ToList();

		result.Should().HaveCount(3);

		context.Entry(result.First())
			   .State
			   .Should()
			   .Be(EntityState.Detached);
	}

	[Fact]
	public async Task GetQuery_should_apply_tracking_when_enabled()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var specification = new TestQuerySpecification();
		specification.EnableTracking();

		var result = _evaluator
					 .GetQuery(context.TestEntities, specification)
					 .ToList();

		result.Should().HaveCount(3);

		context.Entry(result.First())
			   .State
			   .Should()
			   .Be(EntityState.Unchanged);
	}

	[Fact]
	public void GetQuery_should_enable_split_query_when_specified()
	{
		using var context = CreateContext();

		var specification = new TestQuerySpecification();

		specification.AddInclude(x => x.Item);
		specification.EnableSplitQuery();

		var query = _evaluator
			.GetQuery(context.TestEntities, specification);

		query.ToQueryString()
			 .Should()
			 .Contain("split-query mode");
	}

	[Fact]
	public void GetQuery_should_use_single_query_by_default()
	{
		using var context = CreateContext();

		var specification = new TestQuerySpecification();

		specification.AddInclude(x => x.Item);

		var query = _evaluator
			.GetQuery(context.TestEntities, specification);

		query.ToQueryString()
			 .Should()
			 .NotContain("split-query mode");
	}

	[Fact]
	public async Task GetQuery_should_return_all_entities_when_specification_has_no_configuration()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var specification = new TestQuerySpecification();

		var result = _evaluator
					 .GetQuery(context.TestEntities, specification)
					 .ToList();

		result.Should().HaveCount(3);

		result
			.Select(x => x.Id)
			.Should()
			.BeEquivalentTo([1, 2, 3]);
	}

	[Fact]
	public async Task GetQuery_should_apply_all_specification_configurations()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var specification = new TestQuerySpecification();

		specification.SetCriteria(x => x.Id > 0);
		specification.AddInclude(x => x.Item);
		specification.SetOrderBy(x => x.Id, false);
		specification.SetPaging(1, 1);

		var result = _evaluator
					 .GetQuery(context.TestEntities, specification)
					 .ToList();

		result.Should().ContainSingle();

		var entity = result.Single();

		entity.Id.Should().Be(2);
		entity.Item.Should().NotBeNull();
		entity.Item.Id.Should().Be(2);
	}

	[Fact]
	public async Task GetQuery_should_apply_projection()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var specification = new TestProjectionSpecification();

		specification.SetSelector(x => new TestResult { Id = x.Id, Name = x.Name });

		var result = _evaluator
					 .GetQuery(context.TestEntities, specification)
					 .ToList();

		result.Should().HaveCount(3);

		result.Should()
			  .BeEquivalentTo(
			  [
				  new TestResult { Id = 1, Name = "Ali" }, new TestResult { Id = 2, Name = "Reza" },
				  new TestResult { Id = 3, Name = "John" }
			  ]);
	}

	[Fact]
	public async Task GetQuery_should_apply_projection_with_criteria_and_paging()
	{
		await using var context = CreateContext();

		await SeedAsync(context);

		var specification = new TestProjectionSpecification();

		specification.SetSelector(x => new TestResult { Id = x.Id, Name = x.Name });

		specification.SetCriteria(x => x.Id > 1);
		specification.SetOrderBy(x => x.Id);
		specification.SetPaging(0, 1);

		var result = _evaluator
					 .GetQuery(context.TestEntities, specification)
					 .ToList();

		result.Should().ContainSingle();

		result
			.Single()
			.Should()
			.BeEquivalentTo(
							new TestResult { Id = 2, Name = "Reza" });
	}

	private static TestDbContext CreateContext()
	{
		var options = new DbContextOptionsBuilder<TestDbContext>()
					  .UseSqlite("DataSource=:memory:")
					  .Options;

		var context = new TestDbContext(options);

		context.Database.OpenConnection();
		context.Database.EnsureCreated();

		return context;
	}

	private static async Task SeedAsync(TestDbContext context)
	{
		context.TestEntities.AddRange(
									  new TestEntity
									  {
										  Id = 1, Name = "Ali", Item = new InsideTestEntity { Id = 1, Value = 1 }
									  },
									  new TestEntity
									  {
										  Id = 2, Name = "Reza", Item = new InsideTestEntity { Id = 2, Value = 2 }
									  },
									  new TestEntity
									  {
										  Id = 3, Name = "John", Item = new InsideTestEntity { Id = 3, Value = 3 }
									  });

		await context.SaveChangesAsync();
	}

	private sealed class TestDbContext(
		DbContextOptions<TestDbContext> options)
		: DbContext(options)
	{
		public DbSet<TestEntity> TestEntities => Set<TestEntity>();
	}

	private sealed class TestEntity
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public InsideTestEntity Item { get; set; } = null!;
	}

	private sealed class InsideTestEntity
	{
		public int Id { get; set; }

		public double Value { get; set; }
	}

	private sealed class TestResult
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;
	}

	private sealed class TestQuerySpecification
		: QuerySpecification<TestEntity>
	{
		public void SetCriteria(Expression<Func<TestEntity, bool>> criteria)
		{
			Criteria = criteria;
		}

		public void AddInclude(Expression<Func<TestEntity, object>> include)
		{
			ApplyInclude(include);
		}

		public void SetOrderBy(Expression<Func<TestEntity, object>> orderBy,
							   bool isAscending = true)
		{
			ApplyOrderBy(orderBy, isAscending);
		}

		public void SetPaging(int skip,
							  int take)
		{
			ApplyPaging(skip, take);
		}

		public void EnableTracking()
		{
			base.EnableTracking();
		}

		public void EnableSplitQuery()
		{
			ApplySplitQuery();
		}
	}

	private sealed class TestProjectionSpecification
		: QuerySpecification<TestEntity, TestResult>
	{
		public void SetCriteria(Expression<Func<TestEntity, bool>> criteria)
		{
			Criteria = criteria;
		}

		public void SetOrderBy(Expression<Func<TestEntity, object>> orderBy,
							   bool isAscending = true)
		{
			ApplyOrderBy(orderBy, isAscending);
		}

		public void SetPaging(int skip,
							  int take)
		{
			ApplyPaging(skip, take);
		}

		public void SetSelector(Expression<Func<TestEntity, TestResult>> selector)
		{
			ApplySelector(selector);
		}
	}
}