---
source:
  - Fülbier & Sellhorn (2023) | Huang et al. (2024) | Mashiko et al. (2025)
phase: fundamentals
status: draft
last-updated: 2026-03-31
applied-in-project: yes
---

# Lesson 03: Double-Entry "Physics" (Mathematical Integrity)

## Objective
Master the unbreakable law of accounting: **The Accounting Equation**. We treat this as a physical law of our system to ensure 100% financial accuracy.

## Why It Matters for the Ledger
- **Zero Loss**: Money cannot be "created" or "destroyed" inside the ledger; it only moves.
- **Error Detection**: If Debits ≠ Credits, the system must immediately stop (Panic).
- **Precision**: Using the wrong data types (like floats) can cause tiny errors that bankrupt a company over time.

## Key Concepts

### 1. The Dual-Entry Axiom
Every transaction must have at least two entries:
- **Debit (DR)**: An entry on the left side.
- **Credit (CR)**: An entry on the right side.
- **The Law**: `Total Debits - Total Credits = 0`.

### 2. The Accounting Equation
`Assets = Liabilities + Equity`
In our ledger, every "Move" must maintain this balance. If you increase an Asset (Cash), you must either decrease another Asset or increase a Liability/Equity.

### 3. Integer Precision (The "No Float" Rule)
Never use `float` or `double` for money.
- **Why**: `0.1 + 0.2` in binary floating point is `0.30000000000000004`. 
- **The Solution**: Store everything as **Integers** in the smallest unit (e.g., Cents or Basis Points). 
- **Example**: `$100.50` becomes `10050`.

## Mental Model: The Hydraulic Analogy
Think of a closed hydraulic system:
- **The Ledger**: The pipes and valves.
- **The Money**: The hydraulic fluid.
- **The Physics**: If you push 1 liter of fluid out of **Piston A** (Credit), exactly 1 liter must enter **Piston B** (Debit). If only 0.9 liters arrive, you have a leak (System Error).

## Applied Example (A Journal Entry)
Scenario: Company A pays $500 to Company B.

| Account | Debit (DR) | Credit (CR) |
| :--- | :--- | :--- |
| Company A (Liability/Asset) | | 50000 |
| Company B (Liability/Asset) | 50000 | |
| **Total** | **50000** | **50000** |
**Result**: `50000 - 50000 = 0`. The transaction is valid and can be committed.

## Common Pitfalls
- **One-Sided Entries**: "I'll just add $100 to this account." **NO.** Where did it come from? You must have a source account.
- **Rounding Errors**: Rounding at the end of a calculation instead of at every step.
- **Floating Point**: Using `decimal` in C# is better, but many high-performance systems prefer raw `long` (integers) for maximum speed and zero ambiguity.

## Interview Notes
- **Double-Entry Bookkeeping**: A system where every entry to an account requires a corresponding and opposite entry to a different account.
- **Atomic Transaction**: In database terms, the Debit and Credit must happen together or not at all.
- **Rounding Bias**: The cumulative effect of rounding errors.

## Sources
- [[fulbier_2023|Fülbier & Sellhorn, 2023]]: On the "Dual-Entry Axiom" as the core of financial systems.
- [[huang_2024|Huang et al., 2024]]: The importance of Integer Arithmetic in high-frequency ledgers.
- [[mashiko_2025|Mashiko et al., 2025]]: Formal verification of double-entry integrity.

## TODO to Internalize
- [ ] Write a C# code snippet that proves `0.1 + 0.2 != 0.3` using `double`.
- [ ] Create a 3-entry transaction (e.g., $100 payment with a $2 fee) and ensure it balances to zero.
- [ ] Explain why "Equity" is the "Buffer" in the `Assets = Liabilities + Equity` equation.
