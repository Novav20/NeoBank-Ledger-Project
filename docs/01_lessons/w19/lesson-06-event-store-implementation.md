---
source: [Self-study]
phase: [architecture | backend-core]
status: [draft]
last-updated: 2026-05-06
applied-in-project: [yes]
---

# Lesson 19.06: Event Store Implementation and Monotonic Sequencing

## Objective
This lesson explains the event-store implementation that was added after the conceptual sequencing lesson. It shows how the Application layer talks to `IEventStore`, how `RocksDbEventStore` assigns a global sequence number, how `RocksDbStore` keeps the durable metadata, and how the write side now saves audit blocks.

## Why It Matters for the Ledger
- Event sourcing only works if every event has a stable order.
- Replay must be deterministic across restarts.
- The system needs a durable source of truth for the last committed sequence.
- Audit block persistence must stay aligned with the same write model.

## Key Concepts
- `IEventStore` as the Application contract
- Monotonic sequencing with `Interlocked.Increment`
- Durable metadata in `meta:last_sequence`
- Atomic append with `WriteBatch`
- Ordered replay with RocksDB iterators
- Placeholder audit block hashing with SHA256

## Mental Model
```mermaid
flowchart TD
    App[Application Layer] --> Contract[IEventStore]
    Contract --> Store[RocksDbEventStore]
    Store --> Counter[Interlocked counter]
    Counter --> Batch[WriteBatch]
    Batch --> Rocks[RocksDbStore]
    Rocks --> Meta[meta:last_sequence]
    Rocks --> EventKey[evt:{SequenceNumber:D20}]
    Store --> Replay[Iterator range scan]
    App --> UoW[RocksDbLedgerUnitOfWork]
    UoW --> Audit[aud:{BlockHeight:D20}]
```

## Guided Review of the Implementation

### 1. The Application Contract
`IEventStore.cs` defines the event-sourcing operations used by the Application layer.

It exposes three methods:
- `GetNextSequenceAsync()` returns the next global sequence number.
- `AppendAsync(Event eventEntity)` persists a new ordered event.
- `GetEventsAsync(long start, long end)` replays a range of stored events.

Why this matters:
- The Application layer does not know about RocksDB keys.
- The interface keeps the ordering concern in one place.
- Tests can mock the event store without loading Infrastructure.

### 2. Durable Sequence State in `RocksDbStore.cs`
`RocksDbStore.cs` now owns the persisted sequence metadata.

At startup it:
- opens the RocksDB database,
- reads `meta:last_sequence`,
- parses the value into `_lastSequence`,
- falls back to `0` if the key is missing.

It also exposes the helpers that the event store needs:
- `GetLastSequence()` and `SetLastSequence(long seq)`,
- `BuildEventKey(long sequenceNumber)`,
- `BuildLastSequenceKey()`,
- `Database` for iterator-based replay.

Why this is the right place:
- the key schema stays centralized,
- the durable counter survives restarts,
- the low-level adapter remains the single storage authority.

### 3. The Append Flow in `RocksDbEventStore.cs`
`RocksDbEventStore.cs` is the monotonic sequencer.

The append path works like this:
1. Read the current sequence from the shared store state.
2. Increment the in-memory value with `Interlocked.Increment`.
3. Build a new `Event` with the assigned sequence number.
4. Convert the event to `EventDto`.
5. Put the event and `meta:last_sequence` into one `WriteBatch`.
6. Write the batch through `RocksDbStore`.
7. Update the in-memory counter after the commit.

That design gives you:
- monotonic numbering,
- atomic persistence,
- a simple recovery path after restart.

The stored event keeps the full domain shape:
- `EventId`
- `SequenceNumber`
- `CorrelationId`
- `UTI`
- `Timestamp`
- `Precision`
- `DeliveryOrder`
- `DeliveryAssurance`
- `PayloadJson`
- `NonRepudiationProof`
- `ShardId`
- `ConsensusZoneId`

### 4. Replay by Ordered Range
`GetEventsAsync(long start, long end)` uses a RocksDB iterator instead of scanning the full database.

The method:
- builds `evt:{start:D20}` and `evt:{end:D20}` keys,
- seeks to the start key,
- walks forward while the iterator remains valid,
- stops after the end key,
- deserializes each `EventDto` back into an `Event`.

Why this works well:
- the zero-padded sequence keeps lexicographic order aligned with numeric order,
- the iterator reads only the range we need,
- replay becomes predictable and efficient.

### 5. Why the Implementation Uses `Task.FromResult` and `Task.Run`
The RocksDB wrapper used in this project is synchronous.

The event-store implementation keeps an async-friendly API by wrapping work like this:
- `GetNextSequenceAsync()` returns `Task.FromResult(...)` because it only reads the in-memory counter.
- `AppendAsync(...)` and `GetEventsAsync(...)` use `Task.Run(...)` because they perform synchronous storage work and need to preserve the async contract.

If this looked unusual, that is normal. The important point is that the interface stays asynchronous for the rest of the application, even though the underlying database calls are blocking.

### 6. Audit Block Persistence in the Write Side
`RocksDbLedgerUnitOfWork.cs` now includes `SaveAuditBlockAsync(AuditBlock auditBlock)`.

That method:
- prepares a block by hashing a payload with SHA256,
- stores the block as `AuditBlockDto`,
- writes it under `aud:{BlockHeight:D20}`.

This is a practical bridge step:
- it gives the write side a real audit-block persistence path,
- it keeps the shape aligned with the current ADRs,
- it leaves room for the stronger Chameleon Hash implementation later.

### 7. Mapping and DTO Boundaries
The event-store code uses the same static mapping approach as the rest of the persistence layer.

The relevant pieces are:
- `EventMapper.cs`
- `EventDto.cs`
- `AuditBlockDto.cs`
- `RocksDbStore.cs`

Why this matters:
- the domain model stays independent from JSON shape,
- the persistence layer can evolve without rewriting the application logic,
- the conversion stays explicit and compiler-checked.

## Applied Example (.NET 10 / C# 14)
```csharp
Event appended = await eventStore.AppendAsync(eventEntity);
IEnumerable<Event> replay = await eventStore.GetEventsAsync(1, appended.SequenceNumber);

await ledgerUnitOfWork.SaveAuditBlockAsync(auditBlock);
```

What happens here:
- the event store assigns the next sequence and writes the event atomically,
- the replay method can reconstruct the ordered stream from RocksDB,
- the write side can persist an audit block using the same storage adapter.

## Common Pitfalls
- Treating the event store like a plain repository instead of an ordered log.
- Forgetting to persist the updated sequence together with the event.
- Using non-padded keys, which break ordered replay.
- Scanning the whole database instead of using a bounded iterator range.
- Mixing the replay path with the write-side Unit of Work responsibilities.

## Interview Notes
- Event sourcing depends on a deterministic order, not just durable storage.
- `meta:last_sequence` is the durable source of truth for the highest committed event.
- `WriteBatch` keeps the event and its sequencing metadata atomic.
- RocksDB iterators are the right tool for ordered range replay.
- SHA256 is currently a placeholder for the later audit-sealing mechanism.

## Sources
- `IEventStore.cs`
- `RocksDbEventStore.cs`
- `RocksDbStore.cs`
- `RocksDbLedgerUnitOfWork.cs`
- `EventDto.cs`
- `EventMapper.cs`
- `AuditBlockDto.cs`
- `ADR-010-Monotonic-Sequencing-Strategy.md`
- `lesson-05-event-sourcing-and-sequencer.md`

## TODO to Internalize
- [ ] Rewrite from memory
- [ ] Trace the append path in code
- [ ] Trace the replay path in code
- [ ] Explain the sequencing model in your own words