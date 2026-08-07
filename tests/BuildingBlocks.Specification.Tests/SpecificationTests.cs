using System.Linq.Expressions;
using BuildingBlocks.Specification.Base;
using FluentAssertions;

namespace BuildingBlocks.Specification.Tests;

public class SpecificationTests
{
	[Fact]
	public void IsSatisfiedBy_should_return_true_when_entity_satisfies_specification()
	{
		var specification = new IdIsPositiveSpecification();
		var entity = new TestEntity { Id = 1 };

		var result = specification.IsSatisfiedBy(entity);

		result.Should().BeTrue();
	}

	[Fact]
	public void IsSatisfiedBy_should_return_false_when_entity_does_not_satisfy_specification()
	{
		var specification = new IdIsPositiveSpecification();
		var entity = new TestEntity { Id = 0 };

		var result = specification.IsSatisfiedBy(entity);

		result.Should().BeFalse();
	}


	[Fact]
	public void ToExpression_should_return_specification_expression()
	{
		var specification = new IdIsPositiveSpecification();

		var expression = specification.ToExpression();

		var predicate = expression.Compile();

		predicate(new TestEntity { Id = 0 }).Should().BeFalse();
		predicate(new TestEntity { Id = 1 }).Should().BeTrue();
	}


	[Fact]
	public void Should_implicitly_convert_to_expression()
	{
		Expression<Func<TestEntity, bool>> expression = new IdIsPositiveSpecification();

		var predicate = expression.Compile();

		predicate(new TestEntity { Id = 0 }).Should().BeFalse();
		predicate(new TestEntity { Id = 1 }).Should().BeTrue();
	}


	[Fact]
	public void And_should_combine_specifications_with_logical_and()
	{
		var idSpecification = new IdIsPositiveSpecification();
		var nameSpecification = new NameIsNotNullOrEmptySpecification();

		var firstEntity = new TestEntity { Id = 0, Name = "" };
		var secondEntity = new TestEntity { Id = 1, Name = "" };
		var thirdEntity = new TestEntity { Id = 0, Name = "TestName" };
		var fourthEntity = new TestEntity { Id = 1, Name = "TestName" };


		var specification = idSpecification.And(nameSpecification);

		var firstResult = specification.IsSatisfiedBy(firstEntity);
		var secondResult = specification.IsSatisfiedBy(secondEntity);
		var thirdResult = specification.IsSatisfiedBy(thirdEntity);
		var fourthResult = specification.IsSatisfiedBy(fourthEntity);

		firstResult.Should().BeFalse();
		secondResult.Should().BeFalse();
		thirdResult.Should().BeFalse();
		fourthResult.Should().BeTrue();
	}


	[Fact]
	public void Or_should_combine_specifications_with_logical_or()
	{
		var idSpecification = new IdIsPositiveSpecification();
		var nameSpecification = new NameIsNotNullOrEmptySpecification();

		var firstEntity = new TestEntity { Id = 0, Name = "" };
		var secondEntity = new TestEntity { Id = 1, Name = "" };
		var thirdEntity = new TestEntity { Id = 0, Name = "TestName" };
		var fourthEntity = new TestEntity { Id = 1, Name = "TestName" };


		var specification = idSpecification.Or(nameSpecification);

		var firstResult = specification.IsSatisfiedBy(firstEntity);
		var secondResult = specification.IsSatisfiedBy(secondEntity);
		var thirdResult = specification.IsSatisfiedBy(thirdEntity);
		var fourthResult = specification.IsSatisfiedBy(fourthEntity);

		firstResult.Should().BeFalse();
		secondResult.Should().BeTrue();
		thirdResult.Should().BeTrue();
		fourthResult.Should().BeTrue();
	}


	[Fact]
	public void Not_should_negate_specification()
	{
		var idSpecification = new IdIsPositiveSpecification();

		var entity = new TestEntity { Id = 1 };

		var specification = idSpecification.Not();

		var result = specification.IsSatisfiedBy(entity);
		result.Should().BeFalse();
	}

	[Fact]
	public void IsSatisfiedBy_should_compile_expression_only_once()
	{
		var specification = new CountingSpecification();

		var firstEntity = new TestEntity { Id = 1 };
		var secondEntity = new TestEntity { Id = 2 };

		specification.IsSatisfiedBy(firstEntity);
		specification.IsSatisfiedBy(secondEntity);

		specification.ToExpressionCallCount.Should().Be(1);
	}


	private sealed class TestEntity
	{
		public int Id { get; init; }
		public string Name { get; init; } = string.Empty;
	}

	private sealed class IdIsPositiveSpecification : Specification<TestEntity>
	{
		public override Expression<Func<TestEntity, bool>> ToExpression()
		{
			return x => x.Id > 0;
		}
	}

	private sealed class NameIsNotNullOrEmptySpecification : Specification<TestEntity>
	{
		public override Expression<Func<TestEntity, bool>> ToExpression()
		{
			return x => !string.IsNullOrWhiteSpace(x.Name);
		}
	}

	private sealed class CountingSpecification : Specification<TestEntity>
	{
		public int ToExpressionCallCount { get; private set; }

		public override Expression<Func<TestEntity, bool>> ToExpression()
		{
			ToExpressionCallCount++;

			return x => x.Id > 0;
		}
	}
}