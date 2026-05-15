---
status: pending
issued_by: Gemini CLI
issued_date: 2026-05-15
response_ref: docs/00_meta/orchestration/responses/w20/08-double-entry-engine-implementation.md
---
# Task: Implement Double-Entry Engine and Domain Hardening

## Task Context
We are implementing the core logic of the ledger. Following **ADR-012**, we need a stateless Domain Service for the Double-Entry Axiom. We are also "Hardening" our Domain Layer by enforcing strict regulatory standards (UTI, LEI, ISIN) and preventing "Mixed Currency" math at the language level.

## Objectives
1.  **Domain Physics (CurrencyAmount.cs)**: Overload `+` and `-` operators to prevent summing different currencies.
2.  **Standards Enforcement**: Update `UTI`, `LEI`, and create `ISIN` Value Objects to enforce ISO lengths and patterns.
3.  **Entity Invariants**: Update `Transaction` to enforce non-negative amounts and ID trimming.
4.  **Double-Entry Engine**: Create the `IDoubleEntryEngine` domain service to validate $\sum \text{Entries} = 0$.

## Constraints & Requirements
- **Location (Service)**: `src/NeoBank.Ledger.Domain/Services/`.
- **Stateless**: The service must NOT talk to the database.
- **Operator Safety**: `CurrencyAmount` operators MUST throw `ArgumentException` if `CurrencyCode` doesn't match.
- **UTI Pattern**: LEI (20 chars) + Unique Suffix (up to 32 chars). Max 52.

## Specific Instructions
1.  **Operator Overloading**: In `CurrencyAmount.cs`, implement `public static CurrencyAmount operator +(CurrencyAmount a, CurrencyAmount b)`. Ensure codes match before summing `MinorUnits`.
2.  **Transaction Invariants**: In `Transaction` constructor, throw `ArgumentException` if `Amount.MinorUnits <= 0` (for new intake transactions).
3.  **Engine Logic**: In `DoubleEntryEngine.Validate`, sum the list of entries using the new `+` operator. If the result is not zero, return `IsValid = false`.

## Implementation Path
- `src/NeoBank.Ledger.Domain/ValueObjects/ISIN.cs` (New)
- `src/NeoBank.Ledger.Domain/ValueObjects/CurrencyAmount.cs` (Updated)
- `src/NeoBank.Ledger.Domain/Entities/Transaction.cs` (Updated)
- `src/NeoBank.Ledger.Domain/Services/DoubleEntryEngine.cs`
