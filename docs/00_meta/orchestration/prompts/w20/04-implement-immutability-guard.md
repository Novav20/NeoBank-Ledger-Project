---
status: pending
issued_by: Gemini CLI
issued_date: 2026-05-12
response_ref: docs/00_meta/orchestration/responses/w20/04-implement-immutability-guard.md
---
# Task: Implement Event Store Immutability Guard

## Task Context
The Ledger's event log must be immutable. We need to implement an "Immutability Guard" in the `RocksDbEventStore` to ensure that once a sequence number is assigned, it can never be overwritten by another event. This serves as a software interlock to protect the integrity of the history.

## Objectives
1.  **Define Domain Exception**: Create a custom exception `EventCollisionException` to represent a violation of the immutability rule.
2.  **Update Event Store**: Modify `RocksDbEventStore.AppendAsync` to check if an event already exists for the next sequence number before attempting the write.
3.  **Verify via Test**: Write an integration test to prove that attempting to overwrite an event sequence throws the `EventCollisionException`.

## Constraints & Requirements
- **Language**: C# 14.
- **Location (Exception)**: `src/NeoBank.Ledger.Domain/Exceptions/EventCollisionException.cs`
- **Location (Logic)**: `src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbEventStore.cs`
- **Efficiency**: Use the existing `RocksDbStore.GetEvent(long)` method (or equivalent) to perform the existence check efficiently.

## Specific Instructions
1.  **Exception Content**: The `EventCollisionException` should include the `SequenceNumber` and a clear message explaining that the log is immutable.
2.  **Append Logic**:
    - Inside `AppendAsync`, after incrementing the counter, check if an event already exists at that sequence.
    - If it exists, throw the `EventCollisionException` **before** opening the `WriteBatch`.
3.  **Test Case**:
    - Name: `AppendAsync_ShouldThrowException_WhenSequenceAlreadyExists`.
    - **Arrange**: Append Event #1 to a real `RocksDbEventStore`.
    - **Act**: Manually manipulate the store or sequencer to attempt appending another Event with the same `SequenceNumber: 1`.
    - **Assert**: Assert that the call throws `EventCollisionException`.

## Implementation Path
- `src/NeoBank.Ledger.Domain/Exceptions/EventCollisionException.cs`
- `src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbEventStore.cs`
- `tests/NeoBank.Ledger.Infrastructure.Tests/Persistence/RocksDbEventStoreTests.cs`
