# Repository Bridge and Unit of Work

## Executive Summary
The persistence bridge between the Application layer and the RocksDB Infrastructure is now implemented. I added the read-side repository interfaces, RocksDB-backed repository implementations, and a unit of work that uses `WriteBatch` for atomic persistence. I also added `TransactionDto.cs`, `EntryDto.cs`, `TransactionMapper.cs`, and `EntryMapper.cs` so the write path has explicit persistence shapes instead of ad hoc JSON.

## Analysis / Findings
- The new interfaces live under `src/NeoBank.Ledger.Application/Persistence/` and follow the segregated persistence pattern from ADR-009.
- The read-side implementations inject `RocksDbStore` and use `Task.FromResult` because the current RocksDB wrapper is synchronous.
- The write-side implementation uses `RocksDbNet.WriteBatch` through `RocksDbLedgerUnitOfWork`, so the transaction record, optional entry records, and balance projections are committed atomically.
- I added `IPartyRepository` and `RocksDbPartyRepository` as well, because ADR-009 explicitly includes party lookups on the read side.
- The current domain `Transaction` entity does not yet expose an entry collection, so the unit of work accepts an optional entries collection. That keeps the API compatible with the current prompt while leaving the batch path ready for the sequencer when entry materialization is added.

## Proposed Plan / Solution
1. Keep read paths thin and storage-agnostic by routing them through the repository interfaces.
2. Keep write paths centralized in `ILedgerUnitOfWork` so multi-entity updates stay atomic.
3. Extend the sequencer later to supply entry batches when the write-side flow produces them.

## Technical Impact
- **Affected Files:** [src/NeoBank.Ledger.Application/Persistence/IAccountRepository.cs](../../../../../src/NeoBank.Ledger.Application/Persistence/IAccountRepository.cs), [src/NeoBank.Ledger.Application/Persistence/IBalanceRepository.cs](../../../../../src/NeoBank.Ledger.Application/Persistence/IBalanceRepository.cs), [src/NeoBank.Ledger.Application/Persistence/IPartyRepository.cs](../../../../../src/NeoBank.Ledger.Application/Persistence/IPartyRepository.cs), [src/NeoBank.Ledger.Application/Persistence/ILedgerUnitOfWork.cs](../../../../../src/NeoBank.Ledger.Application/Persistence/ILedgerUnitOfWork.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbAccountRepository.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbAccountRepository.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbBalanceRepository.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbBalanceRepository.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbPartyRepository.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbPartyRepository.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbLedgerUnitOfWork.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbLedgerUnitOfWork.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/DTOs/TransactionDto.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/DTOs/TransactionDto.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/DTOs/EntryDto.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/DTOs/EntryDto.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/TransactionMapper.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/TransactionMapper.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/EntryMapper.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/EntryMapper.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/RocksDbStore.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/RocksDbStore.cs), [docs/00_meta/plans/w19.md](../../../../plans/w19.md), [docs/00_meta/orchestration/logs/session-state.md](../../logs/session-state.md)
- **New Dependencies:** None
- **Risk Level:** Medium

## Deliverables / Snippets
```csharp
public interface ILedgerUnitOfWork
{
    Task CommitAsync(
        Transaction transaction,
        IReadOnlyCollection<Balance> updatedBalances,
        IReadOnlyCollection<Entry>? entries = null);
}
```

```csharp
public Task CommitAsync(
    Transaction transaction,
    IReadOnlyCollection<Balance> updatedBalances,
    IReadOnlyCollection<Entry>? entries = null)
{
    return Task.Run(() =>
    {
        var batch = new WriteBatch();
        try
        {
            // transaction + optional entries + balances are staged here
            rocksDbStore.Write(batch);
        }
        finally
        {
            if (batch is IDisposable disposableBatch)
            {
                disposableBatch.Dispose();
            }
        }
    });
}
```

The read repositories stay thin, the unit of work centralizes atomic writes, and the write path now has explicit DTOs and mappers for the next sequencer step.
