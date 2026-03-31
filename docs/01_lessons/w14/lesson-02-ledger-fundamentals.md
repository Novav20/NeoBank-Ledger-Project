---
source:
  - Fülbier & Sellhorn (2023) | Kahmann et al. (2023) | Wu et al. (2026)
phase: fundamentals
status: draft
last-updated: 2026-03-31
applied-in-project: yes
---

# Lesson 02: Ledger Fundamentals (Event Log vs. World State)

## Objective
Distinguish between the **History** of what happened (the Log) and the **Current Reality** (the State). This is the foundation of our **Event Sourcing** architecture.

## Why It Matters for the Ledger
- **Auditability**: If you only store the "Current Balance," you don't know *how* you got there.
- **Recovery**: If the database crashes, you can "replay" the log to rebuild the exact state.
- **Integrity**: An immutable log prevents anyone from "silently" changing a balance without a trace.

## Key Concepts

### 1. The Append-Only Log (The "Truth")
In a modern ledger, we never `UPDATE` a row to change a balance. We only `INSERT` new events.
- **Example**: Instead of `Update Account Set Balance = 150`, we record `Event: Deposit +50`.
- **Property**: **Immutability**. Once written, it is never changed (cryptographically chained).

### 2. The World State (The "View")
The **World State** (or Materialized View) is a snapshot of the current balances.
- It is derived from the log.
- **Formula**: `Initial State + Σ(All Events) = Current World State`.
- We use this for fast lookups (e.g., "Does this user have enough money?").

### 3. Event Sourcing vs. CRUD
- **CRUD (Create, Read, Update, Delete)**: Traditional way. High risk of data loss or "ghost" updates.
- **Event Sourcing**: Our way. We store the *intent* (the event) and derive the *result* (the state).

## Mental Model: The Mechatronics Analogy
Think of a **CNC Machine** or a **PLC**:
- **The Log**: The G-Code or Instruction List (The commands given to the motor).
- **The World State**: The current (X, Y, Z) position of the drill bit.
- If you lose power, you don't just guess where the drill is; you look at the instruction log to see where it *should* be.

## Applied Example (Transaction Flow)
1. **Event**: `Transfer { From: A, To: B, Amount: 100 }` is appended to the **Log**.
2. **Processor**: Reads the event.
3. **State Update**: Updates the **World State** table: `Account A: -100`, `Account B: +100`.

## Common Pitfalls
- **Updating the State Directly**: Never update a balance without a corresponding event in the log. The log is the **Source of Truth**.
- **Log Bloat**: Replaying 10 years of events is slow. We use **Snapshots** (periodic saves of the World State) to speed up recovery.

## Interview Notes
- **Event Sourcing**: A pattern where state changes are logged as a sequence of immutable events.
- **Immutability**: The quality of being unchangeable. Essential for financial audit trails.
- **CQRS**: Often paired with Event Sourcing. **Command** (write to log) is separated from **Query** (read from world state).

## Sources
- [[fulbier_2023|Fülbier & Sellhorn, 2023]]: On the transition from siloed DBs to shared immutable logs.
- [[kahmann_2023|Kahmann et al., 2023]]: Event-driven consistency in ledger architectures.
- [[wu_2026|Wu et al., 2026]]: Conflict-aware ordering in append-only systems.

## TODO to Internalize
- [ ] Diagram a "Deposit" event vs. a "CRUD Update" on a whiteboard.
- [ ] Research the term "Snapshotting" in the context of Event Sourcing.
- [ ] Explain why "Deleting" a transaction is impossible in a compliant ledger (Hint: You use a "Reversal Event" instead).
