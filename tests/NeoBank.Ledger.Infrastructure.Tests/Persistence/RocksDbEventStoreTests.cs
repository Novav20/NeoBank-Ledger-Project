using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Domain.Enums;
using NeoBank.Ledger.Domain.Exceptions;
using NeoBank.Ledger.Domain.ValueObjects;
using NeoBank.Ledger.Infrastructure.Persistence;
using NeoBank.Ledger.Infrastructure.Persistence.Repositories;
using Xunit;

namespace NeoBank.Ledger.Infrastructure.Tests.Persistence;

public sealed class RocksDbEventStoreTests : IDisposable
{
    private readonly string databasePath = CreateTemporaryDatabasePath();

    [Fact]
    public async Task AppendAsync_ShouldThrowException_WhenSequenceAlreadyExists()
    {
        // Arrange
        using var rocksDbStore = new RocksDbStore(databasePath);
        var eventStore = new RocksDbEventStore(rocksDbStore);

        Event firstEvent = CreateEvent("UTI-20260512-IMMUT-01", "{\"type\":\"append-1\"}");
        Event collisionEvent = CreateEvent("UTI-20260512-IMMUT-02", "{\"type\":\"append-2\"}");

        await eventStore.AppendAsync(firstEvent);
        Assert.NotNull(rocksDbStore.GetEvent(1));

        rocksDbStore.SetLastSequence(0);
        var collisionEventStore = new RocksDbEventStore(rocksDbStore);

        // Act
        EventCollisionException exception = await Assert.ThrowsAsync<EventCollisionException>(
            () => collisionEventStore.AppendAsync(collisionEvent));

        // Assert
        Assert.Equal(1, exception.SequenceNumber);
        Assert.Contains("immutable", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.NotNull(rocksDbStore.GetEvent(1));
        Assert.Null(rocksDbStore.GetEvent(2));
    }

    [Fact]
    public async Task SaveEvent_ShouldPreserveMillisecondPrecision()
    {
        // Arrange
        using var rocksDbStore = new RocksDbStore(databasePath);
        var eventStore = new RocksDbEventStore(rocksDbStore);

        DateTimeOffset originalTimestamp = new DateTimeOffset(2026, 5, 14, 9, 15, 30, 555, TimeSpan.Zero).AddTicks(1234);
        Event originalEvent = CreateEvent("UTI-20260514-PREC-01", "{\"type\":\"precision-round-trip\"}", originalTimestamp);

        // Act
        Event storedEvent = await eventStore.AppendAsync(originalEvent);
        Event? retrievedEvent = rocksDbStore.GetEvent(storedEvent.SequenceNumber);

        // Assert
        Assert.NotNull(retrievedEvent);
        Assert.Equal(originalTimestamp, retrievedEvent!.Timestamp);
        Assert.Equal(originalTimestamp.Ticks, retrievedEvent.Timestamp.Ticks);
        Assert.Equal(originalTimestamp.Millisecond, retrievedEvent.Timestamp.Millisecond);
        Assert.Equal(TimeSpan.Zero, retrievedEvent.Timestamp.Offset);
    }

    [Fact]
    public async Task GetEventsAsync_ShouldMaintainStrictChronologicalOrder()
    {
        // Arrange
        using var rocksDbStore = new RocksDbStore(databasePath);
        var eventStore = new RocksDbEventStore(rocksDbStore);

        DateTimeOffset firstTimestamp = new DateTimeOffset(2026, 5, 14, 9, 15, 30, 100, TimeSpan.Zero).AddTicks(1);
        DateTimeOffset secondTimestamp = firstTimestamp.AddMilliseconds(1);
        DateTimeOffset thirdTimestamp = secondTimestamp.AddMilliseconds(1);

        Event firstEvent = await eventStore.AppendAsync(CreateEvent("UTI-20260514-ORDER-01", "{\"type\":\"event-1\"}", firstTimestamp));
        Event secondEvent = await eventStore.AppendAsync(CreateEvent("UTI-20260514-ORDER-02", "{\"type\":\"event-2\"}", secondTimestamp));
        Event thirdEvent = await eventStore.AppendAsync(CreateEvent("UTI-20260514-ORDER-03", "{\"type\":\"event-3\"}", thirdTimestamp));

        // Act
        IReadOnlyList<Event> events = [.. await eventStore.GetEventsAsync(1, 3)];

        // Assert
        Assert.Equal([1L, 2L, 3L], events.Select(@event => @event.SequenceNumber));
        Assert.Equal([firstTimestamp, secondTimestamp, thirdTimestamp], events.Select(@event => @event.Timestamp));
        Assert.True(events[0].Timestamp <= events[1].Timestamp && events[1].Timestamp <= events[2].Timestamp);
        Assert.All(events, @event => Assert.Equal(TimeSpan.Zero, @event.Timestamp.Offset));
        Assert.Equal(firstEvent.EventId, events[0].EventId);
        Assert.Equal(secondEvent.EventId, events[1].EventId);
        Assert.Equal(thirdEvent.EventId, events[2].EventId);
    }

    public void Dispose()
    {
        if (Directory.Exists(databasePath))
        {
            Directory.Delete(databasePath, recursive: true);
        }
    }

    private static Event CreateEvent(string uti, string payloadJson) => CreateEvent(uti, payloadJson, DateTimeOffset.UtcNow);

    private static Event CreateEvent(string uti, string payloadJson, DateTimeOffset timestamp) => new(
        Guid.NewGuid(),
        0,
        Guid.NewGuid(),
        new UniqueTransactionIdentifier(uti),
        timestamp,
        TimestampPrecision.Standard1Millisecond,
        DeliveryOrder.FifoOrdered,
        DeliveryAssurance.ExactlyOnce,
        payloadJson,
        true,
        "shard-01",
        "cz-01");

    // TODO: Move to a helper class
    private static string CreateTemporaryDatabasePath()
    {
        string path = Path.Combine(Path.GetTempPath(), "NeoBank.Ledger.Infrastructure.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }
}