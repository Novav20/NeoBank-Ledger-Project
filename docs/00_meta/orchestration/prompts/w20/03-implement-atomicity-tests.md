---
status: pending
issued_by: Gemini CLI
issued_date: 2026-05-12
response_ref: docs/00_meta/orchestration/responses/w20/03-implement-atomicity-tests.md
---
# Task: Implement Atomicity Integration Tests

## Task Context
We need to verify the high-integrity "Atomicity" requirement. Following the **Clean Room Pattern**, we will write an integration test that proves the `RocksDbLedgerUnitOfWork` commits multiple entities as a single atomic unit using a real RocksDB instance.

## Objectives
1.  **Setup Test Project**: Ensure the `tests/` directory is prepared for Integration Tests (using xUnit).
2.  **Clean Room Logic**: Implement a helper method to create a unique temporary directory for each test and a teardown method to delete it.
3.  **Test Atomicity**: Write a test named `CommitAsync_ShouldPersistAllEntitiesAtomicsally`.
    - **Arrange**: Create a real `RocksDbStore`, `RocksDbLedgerUnitOfWork`, a `Transaction`, two `Entries`, and two `Balances`.
    - **Act**: Call `unitOfWork.CommitAsync(...)`.
    - **Assert**: Use the `RocksDbStore.GetAccount/GetBalance/etc.` methods to verify that ALL entities exist on disk.

## Constraints & Requirements
- **Framework**: xUnit.
- **Library**: `RocksDb.Net`.
- **Naming**: Use the `Arrange/Act/Assert` comments.
- **Safety**: Ensure the temporary database folders are deleted even if a test fails (use `IDisposable` or a `try/finally` block).

## Specific Instructions
1.  **Mocking**: Do NOT use Mocks for the `RocksDbStore`. We want to test the real interaction with the disk.
2.  **Key Check**: Verify that the keys in RocksDB match our namespacing strategy (e.g., `txn:`, `ent:`, `bal:`).
3.  **Data Integrity**: Ensure the values (like the Transaction Amount) retrieved from the DB match the original values sent to the `CommitAsync` method.

## Implementation Path
- `tests/NeoBank.Ledger.Infrastructure.Tests/Persistence/RocksDbLedgerUnitOfWorkTests.cs`
