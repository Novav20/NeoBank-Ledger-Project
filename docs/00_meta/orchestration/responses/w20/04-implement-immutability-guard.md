# Event Store Immutability Guard

## Executive Summary
I implemented an immutability guard in `RocksDbEventStore` so an existing event sequence cannot be overwritten. The guard checks the next sequence number before a write batch is opened and raises a domain-specific exception when a collision is detected.

## Analysis / Findings
- `RocksDbEventStore.AppendAsync` previously assumed the next sequence number was always free.
- The event log needs a software interlock because sequence reuse would violate the ledger’s immutability requirement.
- A dedicated exception type makes the failure explicit and keeps the collision rule visible to callers and tests.

## Technical Impact
- Added `EventCollisionException` in `src/NeoBank.Ledger.Domain/Exceptions/`.
- Updated `RocksDbEventStore.AppendAsync` to check `RocksDbStore.GetEvent(nextSequence)` before any `WriteBatch` is created.
- Added `RocksDbEventStoreTests.cs` to prove that a reused sequence throws `EventCollisionException` against a real RocksDB instance.

## Verification
- `dotnet test tests/NeoBank.Ledger.Infrastructure.Tests/NeoBank.Ledger.Infrastructure.Tests.csproj` succeeded.

## Deliverable
- `src/NeoBank.Ledger.Domain/Exceptions/EventCollisionException.cs`
- `src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbEventStore.cs`
- `tests/NeoBank.Ledger.Infrastructure.Tests/Persistence/RocksDbEventStoreTests.cs`

## Outcome
The event store now rejects sequence collisions before write time, preserving the immutability of the event log.