# Atomicity Integration Tests

## Executive Summary
I implemented the RocksDB atomicity integration test path for `RocksDbLedgerUnitOfWork`. The work adds a dedicated xUnit test project, exposes the minimal store read helpers needed to verify persisted transactions and entries, and validates that real disk-backed writes round-trip the expected values.

## Analysis / Findings
- The test needed real disk I/O, so a clean-room temporary database directory was used instead of mocks.
- `RocksDbStore` did not previously expose `GetTransaction` and `GetEntry`, so those read helpers were added to support the verification step.
- The internal key builders and `Database` handle remained internal, so `InternalsVisibleTo` was added for the test assembly to verify namespacing without weakening the production API surface.

## Technical Impact
- Added `tests/NeoBank.Ledger.Infrastructure.Tests` as an xUnit integration test project.
- Added `CommitAsync_ShouldPersistAllEntitiesAtomicsally` to prove the Unit of Work writes a transaction, two entries, and two balances into RocksDB.
- Added `GetTransaction` and `GetEntry` to `RocksDbStore` so the test can read back the persisted values directly.
- Added a friend-assembly bridge so the test can confirm the `txn:`, `ent:`, and `bal:` key prefixes.

## Verification
- `dotnet test tests/NeoBank.Ledger.Infrastructure.Tests/NeoBank.Ledger.Infrastructure.Tests.csproj` succeeded.

## Deliverable
- `tests/NeoBank.Ledger.Infrastructure.Tests/Persistence/RocksDbLedgerUnitOfWorkTests.cs`
- `src/NeoBank.Ledger.Infrastructure/Persistence/RocksDbStore.cs`
- `src/NeoBank.Ledger.Infrastructure/AssemblyInfo.cs`

## Outcome
The ledger now has a real integration test proving atomic persistence against RocksDB, and the test verifies both key namespacing and value integrity.