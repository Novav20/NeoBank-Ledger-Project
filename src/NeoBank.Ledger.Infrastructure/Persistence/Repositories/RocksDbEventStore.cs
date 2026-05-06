using System.Globalization;
using System.Text.Json;
using NeoBank.Ledger.Application.Persistence;
using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Infrastructure.Persistence.DTOs;
using NeoBank.Ledger.Infrastructure.Persistence.Mappers;
using RocksDbNet;

namespace NeoBank.Ledger.Infrastructure.Persistence.Repositories;

public sealed class RocksDbEventStore(RocksDbStore rocksDbStore) : IEventStore
{
    private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.General);
    private long _lastSequence = rocksDbStore.GetLastSequence();

    public Task<long> GetNextSequenceAsync() => Task.FromResult(Interlocked.Increment(ref _lastSequence));

    public Task<Event> AppendAsync(Event eventEntity)
    {
        return Task.Run(() =>
        {
            long nextSequence = Interlocked.Increment(ref _lastSequence);
            Event storedEvent = new(
                eventEntity.EventId,
                nextSequence,
                eventEntity.CorrelationId,
                eventEntity.UTI,
                eventEntity.Timestamp,
                eventEntity.Precision,
                eventEntity.DeliveryOrder,
                eventEntity.DeliveryAssurance,
                eventEntity.PayloadJson,
                eventEntity.NonRepudiationProof,
                eventEntity.ShardId,
                eventEntity.ConsensusZoneId);

            var batch = new WriteBatch();

            try
            {
                EventDto dto = storedEvent.ToDto();
                batch.Put(RocksDbStore.BuildEventKey(nextSequence), JsonSerializer.Serialize(dto, jsonOptions));
                batch.Put(RocksDbStore.BuildLastSequenceKey(), nextSequence.ToString(CultureInfo.InvariantCulture));
                rocksDbStore.Write(batch);
                rocksDbStore.SetLastSequence(nextSequence);
                return storedEvent;
            }
            finally
            {
                if (batch is IDisposable disposableBatch)
                {
                    disposableBatch.Dispose();
                }
            }
        });
    }

    public Task<IEnumerable<Event>> GetEventsAsync(long start, long end)
    {
        return Task.Run(() =>
        {
            if (end < start)
            {
                return Enumerable.Empty<Event>();
            }

            var events = new List<Event>();
            string startKey = RocksDbStore.BuildEventKey(start);
            string endKey = RocksDbStore.BuildEventKey(end);

            using var iterator = rocksDbStore.Database.NewIterator();
            iterator.Seek(startKey);

            while (iterator.IsValid())
            {
                string currentKey = iterator.KeyAsString();

                if (string.CompareOrdinal(currentKey, endKey) > 0)
                {
                    break;
                }

                if (currentKey.StartsWith("evt:", StringComparison.Ordinal))
                {
                    EventDto? dto = JsonSerializer.Deserialize<EventDto>(iterator.ValueAsString(), jsonOptions);
                    if (dto is not null)
                    {
                        events.Add(dto.ToEntity());
                    }
                }

                iterator.Next();
            }

            return events;
        });
    }
}