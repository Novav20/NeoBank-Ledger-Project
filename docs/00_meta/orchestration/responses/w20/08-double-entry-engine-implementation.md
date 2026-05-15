# Double-Entry Engine Implementation

## Executive Summary
I implemented the Double-Entry Engine as a stateless domain service, hardened `CurrencyAmount` arithmetic against mixed currencies, and upgraded the legal and transaction identifiers to enforce the prompt's stricter validation rules.

## Analysis / Findings
- `Transaction` was still accepting a raw `string` for `ISIN`, so the model did not yet enforce instrument identity at the domain boundary.
- `CurrencyAmount` had no arithmetic operators, which meant mixed-currency math could not be rejected centrally.
- `UniqueTransactionIdentifier` and `LegalEntityIdentifier` were only checking length, not pattern shape or normalization.
- The double-entry rule fits the ADR-012 split cleanly as a stateless domain service that validates `sum(entries) = 0`.

## Proposed Plan / Solution
- Introduce an `ISIN` value object with ISO 6166 formatting checks.
- Add guarded `+` and `-` operators to `CurrencyAmount` so arithmetic fails fast when currencies differ.
- Tighten `Transaction` intake invariants to trim identifiers and reject non-positive amounts.
- Implement `IDoubleEntryEngine` / `DoubleEntryEngine` so the domain service converts entry sides to signed amounts and validates the net total.

## Technical Impact
- **Affected Files:** `src/NeoBank.Ledger.Domain/ValueObjects/CurrencyAmount.cs`, `src/NeoBank.Ledger.Domain/ValueObjects/UniqueTransactionIdentifier.cs`, `src/NeoBank.Ledger.Domain/ValueObjects/LegalEntityIdentifier.cs`, `src/NeoBank.Ledger.Domain/ValueObjects/ISIN.cs`, `src/NeoBank.Ledger.Domain/Entities/Transaction.cs`, `src/NeoBank.Ledger.Domain/Services/DoubleEntryEngine.cs`, `src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/TransactionMapper.cs`, `tests/NeoBank.Ledger.Infrastructure.Tests/Persistence/RocksDbEventStoreTests.cs`, `tests/NeoBank.Ledger.Infrastructure.Tests/Persistence/RocksDbLedgerUnitOfWorkTests.cs`, `tests/NeoBank.Ledger.Infrastructure.Tests/Domain/DoubleEntryEngineTests.cs`
- **New Dependencies:** None.
- **Risk Level:** Medium

## Deliverables / Snippets
- Stateless `IDoubleEntryEngine` / `DoubleEntryEngine` domain service
- `ISIN` value object
- Mixed-currency-safe `CurrencyAmount` operators
- Hardened `Transaction` constructor invariants
- Validation coverage for arithmetic, identifier normalization, and double-entry balancing