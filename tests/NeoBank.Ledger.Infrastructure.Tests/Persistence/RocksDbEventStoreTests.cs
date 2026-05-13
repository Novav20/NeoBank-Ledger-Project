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

    public void Dispose()
    {
        if (Directory.Exists(databasePath))
        {
            Directory.Delete(databasePath, recursive: true);
        }
    }

    private static Event CreateEvent(string uti, string payloadJson) => new(
        Guid.NewGuid(),
        0,
        Guid.NewGuid(),
        new UniqueTransactionIdentifier(uti),
        DateTimeOffset.UtcNow,
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