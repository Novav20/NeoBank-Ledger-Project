# Lesson 18.01: Domain Integrity & Rich vs. Anemic Models (Updated)

## 1. The Architectural Debate: Rich vs. Anemic
You noticed that `Transaction.cs` and `Account.cs` have methods like `MarkAsValidated()` or `Close()`. This is a choice between two patterns:

### The Anemic Domain Model (Anti-Pattern)
In many tutorials, you see "Entities" that are just bags of properties (getters and setters). 
- **Pros**: Easy to map to databases.
- **Cons**: The "Logic" leaks into Service classes. You end up with a `TransactionService` that does everything, and the `Transaction` entity becomes a passive data structure. 

### The Rich Domain Model (Our Choice)
In a high-integrity Ledger, we want the **Entity** to own its state.
- **Why?**: If an `Account` is `Closed`, it should be impossible to change its status back to `Active` without a specific business method that validates if that is allowed.
- **Entity Methods**: Handle *State Transitions* (e.g., Changing Status, Updating Validation Level). They ensure the object is always in a valid state.
- **Application Services**: Handle *Orchestration* (e.g., "Get the account from the database, call `Account.Close()`, then save it back"). 

---

## 2. Terminology Bridge (BPA to Code)

| BPA Term | Domain Entity | Meaning in Software |
| :--- | :--- | :--- |
| **Common Semantic Layer** | `Domain Layer` | The shared language between the Bank and the Fintech. |
| **Double-Entry Axiom** | `Entry` / `Transaction` | The rule that money cannot be created/destroyed. $\sum \text{Debits} + \sum \text{Credits} = 0$. |
| **World State** | `Balance` | The current "snapshot" of an account's funds. |
| **Immutable Log** | `Event` | The "History Book." Append-only; no deletions allowed. |
| **Negative Acknowledgement** | `RejectionRecord` | The standard "receipt" for a failed transaction (ISO 20022). |

---

## 3. The Complete Class Catalog (Implementation Review)

Use this as your single source of truth for the work we did today.

### Core Entities (The "Actors" of the Domain)
1. **`Account`**: Represents a financial container.
   - *Key Logic*: Manages status (Active/Suspended/Closed). It uses `init` properties for partitioning fields (ShardId, PartitionKey) because an account cannot "move" to a different shard once created.
2. **`Transaction`**: The Aggregate Root. It represents the *Intent* of a user.
   - *Key Logic*: Tracks `ValidationLevel` (the QC stations) and `Status`. It is the only entity that can produce an `Event`.
3. **`Entry`**: A single line in the ledger (one side of a double-entry).
   - *Update*: We officially classified this as an **Entity** (not a Value Object) because it has a `Guid EntryId` for forensic auditability.

### Supporting Entities (The "Engine" of the Domain)
4. **`Event`**: The finalized, ordered record of a Transaction.
   - *Role*: This is what gets saved to the **Event Store**. It contains the `PayloadJson` which is the "truth" that can be replayed to rebuild the system state.
5. **`Balance`**: A Materialized Projection.
   - *Key Logic*: `ApplyEntry()`. It ensures that balances are only updated by *newer* events (`sequenceNumber > LastApplied`). This prevents double-counting if the system re-runs a log.
6. **`AuditBlock`**: The Cryptographic "Seal."
   - *Role*: Holds the `ChameleonHash` (for GDPR redaction) and the `QuorumCert` (proof of finality). This is what makes the ledger "Evidence-Grade."

### Compliance & Reference
7. **`Party`**: Represents a Legal Entity (Bank, Fintech, Client).
   - *Role*: Validates against `LEI` (ISO 17442) and checks if the party is "Target Market Eligible" under MiFID II.
8. **`RejectionRecord`**: The "Failed" state record.
   - *Role*: If a transaction fails validation level 5 (Rule Valid), we store this record so the user knows *why* (using standard Pacs002 codes).

---

## 4. Why Smallest Currency Units?
In `CurrencyAmount.cs`, we use `long MinorUnits`.
- **Tutorials**: Often use `decimal`.
- **B2B Ledgers**: Use `long` (Integers). 
- **The Reason**: Floating point math (`0.1 + 0.2`) can result in `0.30000000004`. In a ledger with millions of transactions, these tiny errors add up to lost money. By using `long` (e.g., `100` cents instead of `1.00` dollars), the math is always perfect.

---

**Reading Note**: When you audit the code tomorrow, look for the `using` statements at the top. Notice how `NeoBank.Ledger.Domain.Enums` is used everywhere. This is "Clean Architecture" in action—the vocabulary (Enums) and the objects (Entities) are clearly separated but work together.
