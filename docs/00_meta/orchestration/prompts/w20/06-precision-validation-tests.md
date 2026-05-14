---
status: pending
issued_by: Gemini CLI
issued_date: 2026-05-14
response_ref: docs/00_meta/orchestration/responses/w20/06-precision-validation-tests.md
---
# Task: Implement MiFID II Precision Validation Tests

## Task Context
To satisfy MiFID II (RTS 25), we must prove that our persistence layer preserves at least 1-millisecond (1ms) precision. Following **ADR-011**, we will write an integration test that performs a high-precision round-trip against RocksDB.

## Objectives
1.  **Test Temporal Integrity**: Write a test named `SaveEvent_ShouldPreserveMillisecondPrecision`.
2.  **Verify Round-trip**:
    - **Arrange**: Create a real `RocksDbStore` and an `Event` with a manually crafted `DateTimeOffset` that has a specific millisecond value (e.g., `.555Z`).
    - **Act**: Save the event using the `RocksDbEventStore` and read it back.
    - **Assert**: Assert that the `Timestamp` property of the retrieved event has the EXACT same millisecond value as the original.
3.  **Test Deterministic Replay**: Write a second test `GetEventsAsync_ShouldMaintainStrictChronologicalOrder`.
    - **Arrange**: Append three events to the store with timestamps separated by only 1ms.
    - **Act**: Retrieve the range using `GetEventsAsync`.
    - **Assert**: Verify that the events are returned in the correct monotonic sequence (1, 2, 3) and that their timestamps are chronologically ordered.

## Constraints & Requirements
- **Location**: `tests/NeoBank.Ledger.Infrastructure.Tests/Persistence/RocksDbEventStoreTests.cs`.
- **Framework**: xUnit.
- **Safety**: Use the "Clean Room" pattern (temporary directory) for the test database.

## Specific Instructions
1.  **Precision Level**: While MiFID II requires 1ms, we should verify that we actually preserve the full 7 decimal places (100ns ticks) provided by `DateTimeOffset`.
2.  **UTC Check**: Verify that the retrieved timestamp is in **UTC** (its `Offset` should be zero).

## Implementation Path
- `tests/NeoBank.Ledger.Infrastructure.Tests/Persistence/RocksDbEventStoreTests.cs`
