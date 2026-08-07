using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Persistence.Abstractions.Outbox;
using BuildingBlocks.Persistence.Interceptors.Outbox;
using BuildingBlocks.Persistence.Outbox.Entities;
using BuildingBlocks.Persistence.Outbox.Serialization;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace BuildingBlocks.Persistence.Tests.Interceptors;

public class OutboxInterceptorTests
{
	private readonly OutboxInterceptor _interceptor;
	private readonly Mock<IOutboxEventCollector> _mockCollector;
	private readonly Mock<DbContext> _mockContext;
	private readonly Mock<DbSet<OutboxMessage>> _mockDbSet;
	private readonly Mock<IOutboxSerializer> _mockSerializer;

	public OutboxInterceptorTests()
	{
		_mockCollector = new Mock<IOutboxEventCollector>();
		_mockSerializer = new Mock<IOutboxSerializer>();
		_mockContext = new Mock<DbContext>();
		_mockDbSet = new Mock<DbSet<OutboxMessage>>();

		_mockCollector.Setup(c => c.GetEvents()).Returns(new List<IIntegrationEvent>());

		_mockContext.Setup(c => c.Set<OutboxMessage>()).Returns(_mockDbSet.Object);

		_interceptor = new OutboxInterceptor(_mockCollector.Object, _mockSerializer.Object);
	}

	// --- Helper Methods (با استفاده از Named Arguments برای جلوگیری از جابجایی پارامترها) ---

	private DbContextEventData CreateEventData()
	{
		return new DbContextEventData(
									  null!,
									  null!,
									  _mockContext.Object // ✅ اینجا حتماً باید Mock باشد
									 );
	}

	private DbContextEventData CreateNullContextEventData()
	{
		return new DbContextEventData(
									  null!,
									  null!,
									  null! // ✅ اینجا حتماً باید null باشد
									 );
	}

	private SaveChangesCompletedEventData CreateSavedChangesEventData()
	{
		return new SaveChangesCompletedEventData(
												 null!,
												 null!,
												 _mockContext.Object,
												 1
												);
	}

	// ==================== تست‌های سنکرون ====================

	[Fact]
	public void SavingChanges_WhenContextIsNull_ShouldNotAddMessagesAndReturnBaseResult()
	{
		// Arrange
		var eventData = CreateNullContextEventData();
		var result = InterceptionResult<int>.SuppressWithResult(0);

		// Act
		var interceptionResult = _interceptor.SavingChanges(eventData, result);

		// Assert
		interceptionResult.Should().Be(result);
		_mockDbSet.Verify(d => d.AddRange(It.IsAny<IEnumerable<OutboxMessage>>()), Times.Never);
	}

	[Fact]
	public void SavingChanges_WhenNoEvents_ShouldNotAddMessages()
	{
		// Arrange
		var eventData = CreateEventData();
		var result = InterceptionResult<int>.SuppressWithResult(0);

		// Act
		_interceptor.SavingChanges(eventData, result);

		// Assert
		_mockDbSet.Verify(d => d.AddRange(It.IsAny<IEnumerable<OutboxMessage>>()), Times.Never);
	}

	[Fact]
	public void SavingChanges_WhenEventsExist_ShouldSerializeAndAddMessages()
	{
		// Arrange
		var eventData = CreateEventData();
		var result = InterceptionResult<int>.SuppressWithResult(0);

		var event1 = new DummyIntegrationEvent();
		var events = new List<IIntegrationEvent> { event1 };

		// 🔥 در این تست خاص، Setup پیش‌فرض را Override می‌کنیم
		_mockCollector.Setup(c => c.GetEvents()).Returns(events);

		var serializedEvent = new SerializedIntegrationEvent(
															 "DummyIntegrationEvent",
															 "{}",
															 DateTime.UtcNow,
															 "1.0"
															);
		_mockSerializer.Setup(s => s.Serialize(It.IsAny<IIntegrationEvent>())).Returns(serializedEvent);

		// Act
		_interceptor.SavingChanges(eventData, result);

		// Assert
		_mockSerializer.Verify(s => s.Serialize(event1), Times.Once);
		_mockDbSet.Verify(d => d.AddRange(It.Is<IEnumerable<OutboxMessage>>(msgs => msgs.Count() == 1)), Times.Once);
	}

	[Fact]
	public void SavedChanges_ShouldClearCollectorAndReturnBaseResult()
	{
		// Arrange
		var eventData = CreateSavedChangesEventData();
		var result = 5;

		// Act
		var returnedResult = _interceptor.SavedChanges(eventData, result);

		// Assert
		returnedResult.Should().Be(result);
		_mockCollector.Verify(c => c.Clear(), Times.Once);
	}

	// ==================== تست‌های غیرسنکرون ====================

	[Fact]
	public async Task SavingChangesAsync_WhenContextIsNull_ShouldNotAddMessagesAndReturnBaseResult()
	{
		// Arrange
		var eventData = CreateNullContextEventData();
		var result = InterceptionResult<int>.SuppressWithResult(0);

		// Act
		var interceptionResult = await _interceptor.SavingChangesAsync(eventData, result);

		// Assert
		interceptionResult.Should().Be(result);
		_mockDbSet.Verify(d => d.AddRange(It.IsAny<IEnumerable<OutboxMessage>>()), Times.Never);
	}

	[Fact]
	public async Task SavingChangesAsync_WhenNoEvents_ShouldNotAddMessages()
	{
		// Arrange
		var eventData = CreateEventData();
		var result = InterceptionResult<int>.SuppressWithResult(0);

		// Act
		await _interceptor.SavingChangesAsync(eventData, result);

		// Assert
		_mockDbSet.Verify(d => d.AddRange(It.IsAny<IEnumerable<OutboxMessage>>()), Times.Never);
	}

	[Fact]
	public async Task SavingChangesAsync_WhenEventsExist_ShouldSerializeAndAddMessages()
	{
		// Arrange
		var eventData = CreateEventData();
		var result = InterceptionResult<int>.SuppressWithResult(0);

		var event1 = new DummyIntegrationEvent();
		var events = new List<IIntegrationEvent> { event1 };

		_mockCollector.Setup(c => c.GetEvents()).Returns(events);

		var serializedEvent = new SerializedIntegrationEvent(
															 "DummyIntegrationEvent",
															 "{}",
															 DateTime.UtcNow,
															 "1.0"
															);
		_mockSerializer.Setup(s => s.Serialize(It.IsAny<IIntegrationEvent>())).Returns(serializedEvent);

		// Act
		await _interceptor.SavingChangesAsync(eventData, result);

		// Assert
		_mockSerializer.Verify(s => s.Serialize(event1), Times.Once);
		_mockDbSet.Verify(d => d.AddRange(It.Is<IEnumerable<OutboxMessage>>(msgs => msgs.Count() == 1)), Times.Once);
	}

	[Fact]
	public async Task SavedChangesAsync_ShouldClearCollectorAndReturnBaseResult()
	{
		// Arrange
		var eventData = CreateSavedChangesEventData();
		var result = 5;
		var cts = new CancellationTokenSource();

		// Act
		var returnedResult = await _interceptor.SavedChangesAsync(eventData, result, cts.Token);

		// Assert
		returnedResult.Should().Be(result);
		_mockCollector.Verify(c => c.Clear(), Times.Once);
	}

	private class DummyIntegrationEvent : IIntegrationEvent
	{
		public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
		public string Version { get; } = "1.0";
	}
}