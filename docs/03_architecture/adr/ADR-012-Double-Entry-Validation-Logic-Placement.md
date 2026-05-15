---
Status: Proposed
Date: 2026-05-15
Deciders: Gemini CLI / Juan David
Supersedes: None
---

# ADR-012: Double-Entry Validation Logic Placement

## Context
The "Double-Entry Axiom" (BPA 4.1) is the foundational rule of the ledger: every move of value must have equal debits and credits ($\sum = 0$). We need to decide where this validation logic resides within our Clean Architecture.

## Decision
We will separate the validation logic into two distinct layers:

### 1. Entity Invariants (Transaction.cs)
The `Transaction` entity will be responsible for validating its own internal data integrity:
- Valid IDs and UTIs.
- Non-negative amounts.
- Valid currency codes.

### 2. Double-Entry Domain Service (DoubleEntryEngine.cs)
We will create a **Domain Service** responsible for validating the double-entry relationship:
- Ensuring the sum of all generated Entries equals zero.
- Coordinating balance checks (Sufficient Funds) if required.
- Generating the atomic set of `Entry` objects from a `Transaction`.

## Consequences

### Positive
- **Clear Separation of Concerns**: Entities stay focused on data integrity; services stay focused on business orchestration.
- **Stateless Logic**: The `DoubleEntryEngine` can be easily unit-tested in isolation by providing different transaction inputs.
- **Reusability**: Other parts of the system can use the engine to simulate or validate entries without creating a full transaction record.

### Negative
- **Abstraction Overhead**: Requires an additional class and interface.

## Grounding
- **BPA 4.1 (Foundational Principles)**: Identifies the Double-Entry Axiom as a system-wide law.
- **Domain-Driven Design (DDD)**: Recommends Domain Services for logic that doesn't naturally fit within a single entity.
