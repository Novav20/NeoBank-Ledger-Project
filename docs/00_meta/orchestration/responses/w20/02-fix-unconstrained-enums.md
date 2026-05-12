# Strict Enum Constraints Refactor

## Executive Summary
I synchronized the domain model with the constrained vocabularies defined in the Domain Entity Model. The refactor introduces formal enums for the closed value sets, updates the affected domain entities to use them, and propagates the new types through the persistence DTOs and static mappers so RocksDB round-tripping still works cleanly.

## Analysis / Findings
- `Party.cs` was still using a raw string for registration status, even though the DMD constrains that field to a closed lifecycle set.
- `Event.cs` and `Transaction.cs` were still modeling delivery and message function fields as strings instead of formal enums.
- `AuditBlock.cs` and `Balance.cs` were missing properties that exist in the ERD-controlled vocabulary set, so the domain model was incomplete.
- The DTO and mapper layer was already centralized enough that the enum migration could stay localized without touching the storage engine itself.

## Technical Impact
- Added `RegistrationStatus`, `DeliveryOrder`, `DeliveryAssurance`, `MessageFunction`, and `ChangeType` enums in `src/NeoBank.Ledger.Domain/Enums/`.
- Refactored `Party`, `Event`, `Transaction`, `AuditBlock`, and `Balance` to use the new domain enum types.
- Updated `PartyDto`, `EventDto`, `TransactionDto`, `AuditBlockDto`, and `BalanceDto` to preserve the enum fields through serialization.
- Updated the corresponding static mappers so RocksDB persistence and reads continue to round-trip the new types correctly.

## Verification
- `dotnet build src/NeoBank.Ledger.Infrastructure/NeoBank.Ledger.Infrastructure.csproj` succeeded.

## Deliverable
- Updated domain model types in `src/NeoBank.Ledger.Domain/`
- Updated persistence DTOs and mappers in `src/NeoBank.Ledger.Infrastructure/Persistence/`

## Outcome
The constrained vocabularies are now enforced by the type system instead of raw strings, and the persistence path remains consistent with the domain model.