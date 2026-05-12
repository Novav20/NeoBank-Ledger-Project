---
status: pending
issued_by: Gemini CLI
issued_date: 2026-05-12
response_ref: docs/00_meta/orchestration/responses/w20/02-fix-unconstrained-enums.md
---
# Task: Enforce Strict Enum Constraints in Domain Entities

## Task Context
During our "Study Phase," we identified a discrepancy between the **Domain Entity Model** (DMD) and the actual implementation. Several properties (like `RegistrationStatus` in `Party.cs`) are implemented as raw strings despite being constrained to closed options in the DMD's "Controlled Vocabulary Constraints" section.

## Objectives
1.  **Implement Missing Enums**: Create formal C# enums for all constrained vocabularies.
2.  **Refactor Domain Entities**: Replace `string` properties with the new formal Enums to ensure type safety.
3.  **Update Persistence Layer**: Update DTOs and Mappers to support the round-tripping of these new Enums through RocksDB.

## 1. New Enums to Create
Create the following in `src/NeoBank.Ledger.Domain/Enums/`:

| Enum Name | Allowed Values | Source Note |
| :--- | : :--- | :--- |
| **RegistrationStatus** | `ProvisionallyRegistered`, `Registered`, `Obsolete` | ISO 20022 Lifecycle |
| **DeliveryOrder** | `FifoOrdered`, `ExpectedCausalOrder` | ISO 20022 Delivery |
| **DeliveryAssurance** | `ExactlyOnce` | ISO 20022 Assurance |
| **MessageFunction** | `Newm`, `Canc` | ISO 20022 NEWM/CANC |
| **ChangeType** | `Create`, `Amend`, `Delete` | Audit Trail Logic |

## 2. Refactor Entity Properties
Update the following files in `src/NeoBank.Ledger.Domain/Entities/`:

- **Party.cs**: Change `RegistrationStatus` from `string` to `RegistrationStatus` enum.
- **Event.cs**: Change `DeliveryOrder` and `DeliveryAssurance` from `string` to their respective Enums.
- **Transaction.cs**: Change `MessageFunction` from `string` to `MessageFunction` enum.
- **AuditBlock.cs**: Add `RegistrationStatus` and `ChangeType` properties (which were missing in implementation but exist in the ERD).
- **Balance.cs**: Add `RegistrationStatus` property (missing in implementation but exists in the ERD).

## 3. Propagation to Infrastructure
Ensure the following are updated to maintain the persistence path:
- **DTOs**: Update `PartyDto`, `EventDto`, `TransactionDto`, `AuditBlockDto`, and `BalanceDto` to include these fields. (Note: DTOs can store Enums as the Enum type itself since System.Text.Json handles them).
- **Mappers**: Update the static mappers in `src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/` to handle the new Enum types.

## Constraints & Requirements
- **Consistency**: Use the exact names from the DMD (e.g., `ProvisionallyRegistered` mapped from `PROVISIONALLY_REGISTERED`).
- **Rich Domain**: Ensure constructor parameters and private setters are maintained where applicable.
- **Verification**: Run `dotnet build` to ensure the refactoring didn't break any internal dependencies.
