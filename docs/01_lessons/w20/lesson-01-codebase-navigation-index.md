# Lesson 20.01: Codebase Navigation Index

Welcome back, Juan David! This document is your map to understanding the NeoBank Ledger codebase. We have implemented the "Immutable Foundation" of the system. 

To understand the project, read the files in the following **logical order**.

---

## Phase 1: Foundational Architecture (The "Why")
Before looking at code, refresh your memory on the rules we agreed upon.

| Document                                                  | Focus          | Key Concept                                                                  |
| :-------------------------------------------------------- | :------------- | :--------------------------------------------------------------------------- |
| [[Domain-Entity-Model]]                                   | The Blueprint  | Relationships between Transactions, Events, and Entries.                     |
| [[ADR-008-RocksDB-Key-Schema-and-Serialization-Strategy]] | Storage Choice | Why we pivoted to **RocksDB** for performance.                               |
| [[ADR-009-Repository-and-Unit-of-Work-Pattern]]           | Data Access    | The choice of **Segregated Persistence** (Granular Reads vs. Atomic Writes). |
| [[ADR-010-Monotonic-Sequencing-Strategy]]                 | Sequencing     | How we ensure events are recorded in a **Monotonic** (1, 2, 3...) order.     |

---

## Phase 2: The Domain Layer (The Heart)
*Location: `src/NeoBank.Ledger.Domain/`*
This is pure C# logic with zero dependencies. It represents the "Business Truth."

1.  **Value Objects** (`/ValueObjects/`): The smallest data atoms.
    - Read `CurrencyAmount.cs`: Why we use `long` (cents) instead of `decimal`.
2.  **Enums** (`/Enums/`): The vocabulary of the ledger.
    - Read `ValidationLevel.cs`: The 8 stages of quality control for a transaction.
3.  **Entities** (`/Entities/`): The "Rich" objects that own their state.
    - Read `Transaction.cs`: The "Aggregate Root" that starts the process.
    - Read `Account.cs`: How we manage state transitions (Active, Suspended).
    - Read `Balance.cs`: The `ApplyEntry` method that prevents double-counting.

---

## Phase 3: The Application Layer (The Rules)
*Location: `src/NeoBank.Ledger.Application/Persistence/`*
These are the **Interfaces** (Contracts). They tell the system *what* can be done, but not *how*.

1.  **`IAccountRepository.cs`**: Simple read contract for accounts.
2.  **`IEventStore.cs`**: The contract for appending to the immutable log and replaying history.
3.  **`ILedgerUnitOfWork.cs`**: The most important interface. It ensures a Transaction, its Entries, and Balances are saved **atomically** (all or nothing).

---

## Phase 4: The Infrastructure Layer (The Plumbing)
*Location: `src/NeoBank.Ledger.Infrastructure/Persistence/`*
This is the heavy lifting where we talk to **RocksDB**.

1.  **DTOs** (`/DTOs/`): "Flat" versions of our entities for JSON storage.
2.  **Mappers** (`/Mappers/`): Pure C# classes that translate Entities $\leftrightarrow$ DTOs (No AutoMapper!).
3.  **The Low-Level Engine**:
    - Read `RocksDbStore.cs`: Look at the `internal static` key builders (e.g., `acc:`, `evt:`) and how we initialize the sequencer from `meta:last_sequence`.
4.  **The Repository Implementations** (`/Repositories/`):
    - Read `RocksDbEventStore.cs`: Look at `AppendAsync` and how it uses `WriteBatch` to save the event and the counter simultaneously.
    - Read `RocksDbLedgerUnitOfWork.cs`: How we ensure ledger atomicity.

---

## Phase 5: Audit & Integrity (The Compliance)
1.  **`AuditBlock.cs`** (Domain/Entities): The "Seal" of the ledger.
2.  **`RejectionRecord.cs`** (Domain/Entities): How we handle negative acknowledgments (ISO 20022).

---

### **Study Strategy for Today:**
1.  Open the files in the order above.
2.  In `RocksDbStore.cs`, find the **Sequencer Initialization** logic we discussed.
3.  In `RocksDbLedgerUnitOfWork.cs`, identify the **WriteBatch** usage—this is the secret to a balanced ledger.

**I'm here to explain any specific line of code or architectural choice as you read!**
