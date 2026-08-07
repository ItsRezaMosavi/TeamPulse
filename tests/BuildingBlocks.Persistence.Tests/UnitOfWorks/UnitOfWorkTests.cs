using BuildingBlocks.Application.Events;
using BuildingBlocks.Domain.Aggregates;
using BuildingBlocks.Domain.Events;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.UnitOfWorks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Tests.UnitOfWorks;

public class UnitOfWorkTests
{
	[Fact]
	public async Task SaveChangesAsync_should_persist_changes()
	{
		await using var context = CreateContext();
		var dispatcher = new TestEventDispatcher();

		await using var unitOfWork = new UnitOfWork(context, dispatcher);

		var entity = new TestAggregate("Reza");

		context.Set<TestAggregate>().Add(entity);

		var result = await unitOfWork.SaveChangesAsync();

		result.Should().Be(1);

		var persistedEntity = await context.Set<TestAggregate>()
										   .SingleOrDefaultAsync(x => x.Id == entity.Id);

		persistedEntity.Should().NotBeNull();
		persistedEntity!.Name.Should().Be("Reza");
	}

	[Fact]
	public async Task SaveChangesAsync_should_dispatch_domain_events()
	{
		await using var context = CreateContext();
		var dispatcher = new TestEventDispatcher();

		await using var unitOfWork = new UnitOfWork(context, dispatcher);

		var entity = new TestAggregate("Reza");
		var domainEvent = new TestDomainEvent();

		entity.RaiseEvent(domainEvent);

		context.Set<TestAggregate>().Add(entity);

		await unitOfWork.SaveChangesAsync();

		dispatcher.DispatchCallCount.Should().Be(1);
		dispatcher.DispatchedEvents.Should().ContainSingle();
		dispatcher.DispatchedEvents.Single().Should().BeSameAs(domainEvent);
	}

	[Fact]
	public async Task SaveChangesAsync_should_clear_domain_events_after_dispatch()
	{
		await using var context = CreateContext();
		var dispatcher = new TestEventDispatcher();

		await using var unitOfWork = new UnitOfWork(context, dispatcher);

		var entity = new TestAggregate("Reza");
		var domainEvent = new TestDomainEvent();

		entity.RaiseEvent(domainEvent);

		context.Set<TestAggregate>().Add(entity);

		await unitOfWork.SaveChangesAsync();

		entity.DomainEvents.Should().BeEmpty();
	}

	[Fact]
	public async Task SaveChangesAsync_should_not_dispatch_when_there_are_no_domain_events()
	{
		await using var context = CreateContext();
		var dispatcher = new TestEventDispatcher();

		await using var unitOfWork = new UnitOfWork(context, dispatcher);

		var entity = new TestAggregate("Reza");

		context.Set<TestAggregate>().Add(entity);

		await unitOfWork.SaveChangesAsync();

		dispatcher.DispatchCallCount.Should().Be(0);
		dispatcher.DispatchedEvents.Should().BeEmpty();
	}

	[Fact]
	public async Task SaveChangesAsync_should_dispatch_all_domain_events()
	{
		await using var context = CreateContext();
		var dispatcher = new TestEventDispatcher();

		await using var unitOfWork = new UnitOfWork(context, dispatcher);

		var entity = new TestAggregate("Reza");

		var firstEvent = new TestDomainEvent();
		var secondEvent = new TestDomainEvent();

		entity.RaiseEvent(firstEvent);
		entity.RaiseEvent(secondEvent);

		context.Set<TestAggregate>().Add(entity);

		await unitOfWork.SaveChangesAsync();

		dispatcher.DispatchedEvents.Should().HaveCount(2);

		dispatcher.DispatchedEvents.Should()
				  .ContainInOrder(
								  firstEvent,
								  secondEvent);
	}

	[Fact]
	public async Task SaveChangesAsync_should_collect_events_from_multiple_aggregates()
	{
		await using var context = CreateContext();
		var dispatcher = new TestEventDispatcher();

		await using var unitOfWork = new UnitOfWork(context, dispatcher);

		var firstAggregate = new TestAggregate("Ali");
		var secondAggregate = new TestAggregate("Reza");

		var firstEvent = new TestDomainEvent();
		var secondEvent = new TestDomainEvent();

		firstAggregate.RaiseEvent(firstEvent);
		secondAggregate.RaiseEvent(secondEvent);

		context.Set<TestAggregate>()
			   .AddRange(
						 firstAggregate,
						 secondAggregate);

		await unitOfWork.SaveChangesAsync();

		dispatcher.DispatchedEvents.Should().HaveCount(2);

		dispatcher.DispatchedEvents.Should().Contain(firstEvent);
		dispatcher.DispatchedEvents.Should().Contain(secondEvent);
	}

	[Fact]
	public async Task SaveChangesAsync_should_clear_domain_events_from_all_aggregates()
	{
		await using var context = CreateContext();
		var dispatcher = new TestEventDispatcher();

		await using var unitOfWork = new UnitOfWork(context, dispatcher);

		var firstAggregate = new TestAggregate("Ali");
		var secondAggregate = new TestAggregate("Reza");

		firstAggregate.RaiseEvent(new TestDomainEvent());
		secondAggregate.RaiseEvent(new TestDomainEvent());

		context.Set<TestAggregate>()
			   .AddRange(
						 firstAggregate,
						 secondAggregate);

		await unitOfWork.SaveChangesAsync();

		firstAggregate.DomainEvents.Should().BeEmpty();
		secondAggregate.DomainEvents.Should().BeEmpty();
	}

	[Fact]
	public async Task SaveChangesAsync_should_clear_domain_events_when_dispatch_fails()
	{
		await using var context = CreateContext();

		var dispatcher = new TestEventDispatcher { ShouldThrow = true };

		await using var unitOfWork = new UnitOfWork(context, dispatcher);

		var entity = new TestAggregate("Reza");

		entity.RaiseEvent(new TestDomainEvent());

		context.Set<TestAggregate>().Add(entity);

		var action = () => unitOfWork.SaveChangesAsync();

		await action.Should()
					.ThrowAsync<InvalidOperationException>();

		entity.DomainEvents.Should().BeEmpty();
	}

	[Fact]
	public async Task SaveChangesAsync_should_return_number_of_saved_entries()
	{
		await using var context = CreateContext();
		var dispatcher = new TestEventDispatcher();

		await using var unitOfWork = new UnitOfWork(context, dispatcher);

		var firstEntity = new TestAggregate("Ali");
		var secondEntity = new TestAggregate("Reza");

		context.Set<TestAggregate>()
			   .AddRange(
						 firstEntity,
						 secondEntity);

		var result = await unitOfWork.SaveChangesAsync();

		result.Should().Be(2);
	}

	[Fact]
	public async Task SaveChangesAsync_should_not_clear_domain_events_before_dispatch()
	{
		await using var context = CreateContext();
		var dispatcher = new TestEventDispatcher();

		await using var unitOfWork = new UnitOfWork(context, dispatcher);

		var entity = new TestAggregate("Reza");
		var domainEvent = new TestDomainEvent();

		entity.RaiseEvent(domainEvent);

		context.Set<TestAggregate>().Add(entity);

		await unitOfWork.SaveChangesAsync();

		dispatcher.EventsObservedDuringDispatch
				  .Should()
				  .ContainSingle()
				  .Which
				  .Should()
				  .BeSameAs(domainEvent);
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

	private sealed class TestAggregate(string name) : AggregateRoot
	{
		public string Name { get; private set; } = name;

		public void RaiseEvent(IDomainEvent domainEvent)
		{
			AddDomainEvent(domainEvent);
		}
	}

	private sealed record TestDomainEvent : IDomainEvent
	{
		public Guid EventId { get; } = Guid.NewGuid();

		public DateTime OccurredOn { get; } = DateTime.UtcNow;
	}

	private sealed class TestEventDispatcher : IEventDispatcher
	{
		public List<IDomainEvent> DispatchedEvents { get; } = [];

		public List<IDomainEvent> EventsObservedDuringDispatch { get; } = [];

		public int DispatchCallCount { get; private set; }

		public bool ShouldThrow { get; init; }

		public Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents,
								  CancellationToken cancellationToken = default)
		{
			DispatchCallCount++;

			var events = domainEvents.ToList();

			EventsObservedDuringDispatch.AddRange(events);
			DispatchedEvents.AddRange(events);

			if (ShouldThrow)
				throw new InvalidOperationException(
													"Test dispatch failure.");

			return Task.CompletedTask;
		}
	}
}