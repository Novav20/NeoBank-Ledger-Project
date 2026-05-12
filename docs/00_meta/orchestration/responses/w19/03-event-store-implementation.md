# Event Store and Monotonic Sequencer

## Executive Summary
The event store and monotonic sequencer are implemented. `IEventStore.cs` defines the append and replay contract, `RocksDbEventStore.cs` assigns global sequence numbers and appends events atomically, and `RocksDbStore.cs` now tracks `meta:last_sequence` in memory so the sequence state is initialized from the persisted metadata.

## Analysis / Findings
- `ADR-010-Monotonic-Sequencing-Strategy.md` already exists and matches the implementation direction: a reserved `meta:last_sequence` key, an in-memory monotonic counter, and a `WriteBatch` that commits the event and the counter together.
- `RocksDbEventStore.cs` uses `Interlocked.Increment` for sequence assignment, `WriteBatch` for atomic persistence, and RocksDB iterators for range replay between two sequence endpoints.
- `RocksDbStore.cs` now exposes the underlying RocksDB handle internally so the event store can scan efficiently without duplicating low-level database access.
- `SaveAuditBlockAsync` was added to `RocksDbLedgerUnitOfWork.cs` so the write side can prepare for the sealing step. For now, the placeholder hash is a SHA256 digest.

## Proposed Plan / Solution
1. Keep the event store as the single owner of ordered append/replay behavior.
2. Keep the monotonic sequence in metadata so restarts recover the last committed order.
3. Add immutability tests next, because the sequence layer is now in place.

## Technical Impact
- **Affected Files:** `src/NeoBank.Ledger.Application/Persistence/IEventStore.cs`, `src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbEventStore.cs`, `src/NeoBank.Ledger.Infrastructure/Persistence/RocksDbStore.cs`, `src/NeoBank.Ledger.Application/Persistence/ILedgerUnitOfWork.cs`, `src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbLedgerUnitOfWork.cs`, `docs/00_meta/plans/w19.md`, `docs/00_meta/orchestration/logs/session-state.md`
- **New Dependencies:** None
- **Risk Level:** Medium

## Deliverables / Snippets
```csharp
Task<long> GetNextSequenceAsync();
Task<Event> AppendAsync(Event eventEntity);
Task<IEnumerable<Event>> GetEventsAsync(long start, long end);
```

```csharp
batch.Put(RocksDbStore.BuildEventKey(nextSequence), JsonSerializer.Serialize(dto, jsonOptions));
batch.Put(RocksDbStore.BuildLastSequenceKey(), nextSequence.ToString(CultureInfo.InvariantCulture));
```

The event store now owns deterministic replay, the sequence metadata is durable, and the next step is immutability validation.
