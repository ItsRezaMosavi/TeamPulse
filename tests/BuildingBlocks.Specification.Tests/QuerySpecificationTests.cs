using System.Linq.Expressions;
using BuildingBlocks.Specification.Base;
using FluentAssertions;

namespace BuildingBlocks.Specification.Tests;

public class QuerySpecificationTests
{
	[Fact]
	public void ApplyPaging_should_enable_paging_with_valid_values()
	{
		var specification = new TestQuerySpecification();

		specification.SetPaging(4, 2);

		specification.IsPagingEnabled.Should().BeTrue();
		specification.Skip.Should().Be(4);
		specification.Take.Should().Be(2);
	}

	[Fact]
	public void ApplyPaging_should_throw_when_skip_is_negative()
	{
		var specification = new TestQuerySpecification();

		var action = () => specification.SetPaging(-5, 2);

		action.Should().Throw<ArgumentOutOfRangeException>();
	}

	[Fact]
	public void ApplyPaging_should_throw_when_take_is_zero()
	{
		var specification = new TestQuerySpecification();

		var action = () => specification.SetPaging(5, 0);

		action.Should().Throw<ArgumentOutOfRangeException>();
	}

	[Fact]
	public void ApplyPaging_should_throw_when_take_is_negative()
	{
		var specification = new TestQuerySpecification();

		var action = () => specification.SetPaging(5, -3);

		action.Should().Throw<ArgumentOutOfRangeException>();
	}

	[Fact]
	public void ApplyOrderBy_should_set_ordering_expression()
	{
		var specification = new TestQuerySpecification();

		specification.SetOrderBy(x => x.Id);

		specification.OrderBy.Should().NotBeNull();

		var orderBy = specification.OrderBy!.Compile();

		orderBy(new TestEntity { Id = 10 })
			.Should()
			.Be(10);
	}

	[Fact]
	public void ApplyOrderBy_should_default_to_ascending_order()
	{
		var specification = new TestQuerySpecification();

		specification.SetOrderBy(x => x.Id);

		specification.IsAscending.Should().BeTrue();
	}

	[Fact]
	public void ApplyOrderBy_should_set_descending_order()
	{
		var specification = new TestQuerySpecification();

		specification.SetOrderBy(x => x.Id, false);

		specification.IsAscending.Should().BeFalse();
	}

	[Fact]
	public void ApplyInclude_should_add_include_expression()
	{
		var specification = new TestQuerySpecification();

		specification.AddInclude(x => x.TestEntity2);

		specification.Includes.Should().HaveCount(1);

		var include = specification.Includes
								   .Single()
								   .Compile();

		var entity = new TestEntity { TestEntity2 = new TestEntity2 { Id = 10 } };

		include(entity).Should().Be(entity.TestEntity2);
	}

	[Fact]
	public void ApplySplitQuery_should_enable_split_query()
	{
		var specification = new TestQuerySpecification();

		specification.EnableSplitQuery();

		specification.IsSplitQuery.Should().BeTrue();
	}

	[Fact]
	public void Tracking_should_be_disabled_by_default()
	{
		var specification = new TestQuerySpecification();

		specification.IsTrackingEnabled.Should().BeFalse();
	}

	[Fact]
	public void EnableTracking_should_enable_tracking()
	{
		var specification = new TestQuerySpecification();

		specification.EnableTracking();

		specification.IsTrackingEnabled.Should().BeTrue();
	}

	[Fact]
	public void ApplySelector_should_set_projection_expression()
	{
		var specification = new TestProjectionSpecification();

		specification.SetSelector(x => new TestResult { Id = x.Id, Name = x.Name });

		specification.Selector.Should().NotBeNull();

		var selector = specification.Selector.Compile();

		var result = selector(new TestEntity { Id = 10, Name = "Test" });

		result.Id.Should().Be(10);
		result.Name.Should().Be("Test");
	}

	[Fact]
	public void ApplySelector_should_project_entity_to_result()
	{
		var specification = new TestProjectionSpecification();

		specification.SetSelector(x => new TestResult { Id = x.Id });

		var entity = new TestEntity { Id = 42, Name = "Test" };

		var result = specification.Selector.Compile()(entity);

		result.Should().NotBeNull();
		result.Id.Should().Be(42);
	}

	private sealed class TestEntity
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public TestEntity2 TestEntity2 { get; set; } = null!;
	}

	private sealed class TestEntity2
	{
		public int Id { get; set; }
	}

	private sealed class TestResult
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;
	}

	private sealed class TestQuerySpecification
		: QuerySpecification<TestEntity>
	{
		public void SetPaging(int skip, int take)
		{
			ApplyPaging(skip, take);
		}

		public void SetOrderBy(Expression<Func<TestEntity, object>> orderBy,
							   bool isAscending = true)
		{
			ApplyOrderBy(orderBy, isAscending);
		}

		public void AddInclude(Expression<Func<TestEntity, object>> includeExpression)
		{
			ApplyInclude(includeExpression);
		}

		public void EnableSplitQuery()
		{
			ApplySplitQuery();
		}

		public void EnableTracking()
		{
			base.EnableTracking();
		}
	}

	private sealed class TestProjectionSpecification
		: QuerySpecification<TestEntity, TestResult>
	{
		public void SetSelector(Expression<Func<TestEntity, TestResult>> selector)
		{
			ApplySelector(selector);
		}
	}
}