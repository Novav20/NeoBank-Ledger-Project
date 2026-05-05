---
status: pending
issued_by: Gemini CLI
issued_date: 2026-05-05
response_ref: docs/00_meta/orchestration/responses/w19/02-repository-implementation.md
---
# Task: Implement Persistence Repositories and Unit of Work

## Task Context
We are establishing the high-level persistence bridge between the Application core and the Infrastructure implementation. We have decided on a segregated approach: granular interfaces for reads and a unified Unit of Work for atomic writes (ADR-009).

## Objectives
1.  **Define Application Interfaces**: Create repository and unit of work interfaces in the Application project.
2.  **Implement Infrastructure Classes**: Create the RocksDB-backed implementations of these interfaces in the Infrastructure project.

## Constraints & Requirements
- **Location (Interfaces)**: `src/NeoBank.Ledger.Application/Persistence/`
- **Location (Implementations)**: `src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/`
- **Dependency**: Implementations must inject `RocksDbStore` through their constructors.
- **Atomicity**: The Unit of Work MUST use the `RocksDbNet.WriteBatch` API to ensure all entities (Transaction, Entries, and Balances) are committed together in a single atomic operation.

## Specific Instructions
1.  **Interfaces to create**:
    - `IAccountRepository`: `Task<Account?> GetByIdAsync(Guid id);`
    - `IBalanceRepository`: `Task<Balance?> GetByAccountIdAsync(Guid accountId);`
    - `ILedgerUnitOfWork`: A `CommitAsync` method that accepts the `Transaction` aggregate and the list of updated `Balance` projections.
2.  **Mapping**: Use the static mappers (e.g., `AccountMapper`) to convert between Domain Entities and Persistence DTOs inside the implementation classes.
3.  **Naming Convention**: Implementations should be named `RocksDb[Entity]Repository` (e.g., `RocksDbAccountRepository`).
4.  **Async**: Use `Task.FromResult` or `Task.Run` if the underlying RocksDB wrapper is synchronous, to preserve the `Async` contract of the interfaces.

## Implementation Path
- `src/NeoBank.Ledger.Application/Persistence/` (New Folder)
- `src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/` (New Folder)
