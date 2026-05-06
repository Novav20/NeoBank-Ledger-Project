---
status: pending
issued_by: Gemini CLI
issued_date: 2026-05-06
response_ref: docs/00_meta/orchestration/responses/w19/03-event-store-implementation.md
---
# Task: Implement Event Store and Monotonic Sequencer

## Task Context
We are implementing the Event Sourcing core of the Ledger. Following ADR-010, we are building a monotonic sequencer that uses a reserved RocksDB key (`meta:last_sequence`) to track global order.

## Objectives
1.  **Define Application Interface**: Create `IEventStore` in the Application layer.
2.  **Initialize Sequencer in Store**: Update `RocksDbStore` to load the last sequence number into an in-memory thread-safe counter.
3.  **Implement RocksDbEventStore**: Create the implementation that handles `AppendAsync` and `GetSequenceRangeAsync`.
4.  **Implement AuditBlock Logic**: Implement a basic `SaveAuditBlock` method in the `ILedgerUnitOfWork` (or a dedicated repository) to prepare for the "Sealing" step.

## Constraints & Requirements
- **Location (Interface)**: `src/NeoBank.Ledger.Application/Persistence/IEventStore.cs`
- **Location (Implementation)**: `src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbEventStore.cs`
- **Concurrency**: Use `private long _lastSequence` with `Interlocked.Read` and `Interlocked.Increment`.
- **Atomicity**: The `AppendAsync` logic MUST use a `WriteBatch` to save both the Event and the updated `meta:last_sequence` key.
- **Range Scans**: `GetSequenceRangeAsync` should use RocksDB Iterators to efficiently scan keys between `evt:{start:D20}` and `evt:{end:D20}`.

## Specific Instructions
1.  **RocksDbStore Update**: Add a `long GetLastSequence()` and `void SetLastSequence(long seq)` helper to the low-level store.
2.  **IEventStore methods**:
    - `Task<long> GetNextSequenceAsync();`
    - `Task<Event> AppendAsync(Event eventEntity);`
    - `Task<IEnumerable<Event>> GetEventsAsync(long start, long end);`
3.  **AuditBlock**: The `SaveAuditBlock` should use the `BlockHeight:D20` as the key (ADR-008). For now, use a simple `SHA256` hash as a placeholder for the Chameleon Hash.

## Implementation Path
- `src/NeoBank.Ledger.Application/Persistence/IEventStore.cs`
- `src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbEventStore.cs`
- `src/NeoBank.Ledger.Infrastructure/Persistence/RocksDbStore.cs` (Updated)
