# Implement MiFID II Precision Validation Tests

## Executive Summary
I added two RocksDB integration tests that validate MiFID II precision retention and strict chronological replay for persisted events. The suite now proves that `DateTimeOffset` values round-trip with UTC offset zero and full tick-level fidelity, while event retrieval remains monotonic by sequence number.

## Analysis / Findings
- `RocksDbEventStore` already persists `Event.Timestamp` through `EventDto` using `System.Text.Json`, so the right place to validate precision is the integration test layer.
- The clean-room test pattern was preserved by using a temporary RocksDB directory per test class instance.
- The new precision test asserts the round-tripped `DateTimeOffset` value, its raw ticks, its millisecond component, and its UTC offset.
- The replay-order test confirms the store returns sequences `1, 2, 3` and that timestamps remain strictly chronological when only 1ms apart.

## Proposed Plan / Solution
- Keep precision validation as a dedicated integration test slice in `RocksDbEventStoreTests.cs`.
- Use explicit `DateTimeOffset` construction with UTC offsets in test data so the precision boundary is unambiguous.
- Reuse the existing clean-room RocksDB setup so the tests stay isolated and deterministic.

## Technical Impact
- **Affected Files:** `tests/NeoBank.Ledger.Infrastructure.Tests/Persistence/RocksDbEventStoreTests.cs`, `docs/03_architecture/adr/ADR-011-High-Precision-Timestamp-Serialization.md`, `docs/01_lessons/w20/lesson-05-temporal-integrity-compliance.md`, `docs/00_meta/orchestration/logs/session-state.md`, `docs/00_meta/orchestration/logs/weekly/05/w20/2026-05-14.md`, `docs/00_meta/plans/05/w20.md`
- **New Dependencies:** None.
- **Risk Level:** Low.

## Deliverables / Snippets
- `tests/NeoBank.Ledger.Infrastructure.Tests/Persistence/RocksDbEventStoreTests.cs`
- `docs/03_architecture/adr/ADR-011-High-Precision-Timestamp-Serialization.md`
- `docs/01_lessons/w20/lesson-05-temporal-integrity-compliance.md`

## Verification
- `dotnet test tests/NeoBank.Ledger.Infrastructure.Tests/NeoBank.Ledger.Infrastructure.Tests.csproj --filter FullyQualifiedName~RocksDbEventStoreTests` succeeded.

## Outcome
MiFID II precision and deterministic replay are now covered by integration tests against a real RocksDB store, with UTC preservation and tick-level fidelity explicitly verified.