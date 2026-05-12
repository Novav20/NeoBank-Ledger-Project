# Static Mappers and RocksDbStore

## Executive Summary
The RocksDB persistence path is implemented for the Infrastructure layer. The old LevelDB package was removed from the Infrastructure project, `RocksDb.Net` was installed, static extension mappers were added for the persisted entities, and `RocksDbStore` now opens the database in its constructor and persists DTOs as JSON using the ADR-008 key schema.

## Analysis / Findings
- The NuGet restore resolved `RocksDb.Net` 11.0.4.2 plus `RocksDb.Net.Runtimes`, and the package exposes the `RocksDbNet` namespace with the string-based `RocksDb.Open`, `Put`, and `GetString` APIs needed for this task.
- The store uses entity-specific read methods such as `GetAccount`, `GetEvent`, and `GetAuditBlock` because several entities use `Guid` identifiers. A single overloaded `Get(Guid)` would be ambiguous for `Account`, `Balance`, and `Party`.
- `Party` had one persistence gap: `RegistrationStatus` was private-set with no constructor path. I added an optional constructor parameter so the mapper can round-trip the DTO value without resorting to reflection.
- The Week 19 plan and live session snapshot were updated so the repository no longer describes this work as LevelDB-based.

## Proposed Plan / Solution
1. Keep the mapping layer explicit and static, with one mapper per entity/DTO pair.
2. Persist DTOs as JSON strings through RocksDB using ADR-008 key prefixes.
3. Continue the Week 19 persistence work from this RocksDB baseline, starting with repository interfaces and then the sequencer.

## Technical Impact
- **Affected Files:** [src/NeoBank.Ledger.Infrastructure/NeoBank.Ledger.Infrastructure.csproj](../../../../../src/NeoBank.Ledger.Infrastructure/NeoBank.Ledger.Infrastructure.csproj), [src/NeoBank.Ledger.Infrastructure/Persistence/RocksDbStore.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/RocksDbStore.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/AccountMapper.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/AccountMapper.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/EventMapper.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/EventMapper.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/BalanceMapper.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/BalanceMapper.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/PartyMapper.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/PartyMapper.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/AuditBlockMapper.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/AuditBlockMapper.cs), [src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/RejectionRecordMapper.cs](../../../../../src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/RejectionRecordMapper.cs), [src/NeoBank.Ledger.Domain/Entities/Party.cs](../../../../../src/NeoBank.Ledger.Domain/Entities/Party.cs), [docs/00_meta/plans/w19.md](../../../../plans/w19.md), [docs/00_meta/orchestration/logs/session-state.md](../../../../orchestration/logs/session-state.md)
- **New Dependencies:** `RocksDb.Net` 11.0.4.2, `RocksDb.Net.Runtimes` 11.0.4.4
- **Risk Level:** Low

## Deliverables / Snippets
```csharp
public void Save(Account account) => Put(BuildAccountKey(account.AccountId), account.ToDto());

public Account? GetAccount(Guid accountId) =>
    Get<AccountDto>(BuildAccountKey(accountId))?.ToEntity();
```

The same pattern is repeated for `Event`, `Balance`, `Party`, `AuditBlock`, and `RejectionRecord`, with ADR-008-compliant key prefixes and `System.Text.Json` serialization.