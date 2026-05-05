---
Status: Proposed
Date: 2026-05-05
Deciders: Gemini CLI / Juan David
Supersedes: None
---

# ADR-009: Repository and Unit of Work Patterns

## Context
A high-integrity ledger requires strict **Atomicity** for all state-changing operations. A single business transaction involves updating multiple entities (Transaction, Entries, and Balances). If these updates are performed across multiple independent repositories, we risk partial failures that would leave the ledger in an inconsistent state (e.g., recorded entries without updated balances). We also need a clean abstraction for the Application Layer to read data without knowing about the underlying RocksDB implementation.

## Decision
We will adopt a **Segregated Persistence Pattern** consisting of specific Read-Only Repositories and a unified Unit of Work for writes.

### 1. Granular Read Repositories
We will define specific interfaces in the **Application Layer** for reading entities.
- `IAccountRepository`: To retrieve Account details and metadata.
- `IBalanceRepository`: To retrieve the current world state of an account.
- `IPartyRepository`: To verify legal entity eligibility.

### 2. Unified Unit of Work (ILedgerUnitOfWork)
To ensure atomicity, all write operations will be channeled through a single **Unit of Work** interface.
- This service will leverage RocksDB's `WriteBatch` capability.
- It will accept a complete set of changes (e.g., the Transaction aggregate and the corresponding Balance projections) and commit them in a single atomic disk operation.

## Consequences

### Positive
- **Guaranteed Consistency**: Eliminates the risk of "partial writes" where balances and transactions get out of sync.
- **Clean Abstraction**: The Application Layer remains "Storage Agnostic," facilitating easier testing and future database migrations.
- **Performance**: RocksDB `WriteBatch` is more efficient than multiple individual `Put` operations.

### Negative
- **Abstraction Overhead**: Requires creating more interface and implementation classes.
- **Complex Writes**: Writing a single entity (like a simple metadata update) must still follow the Unit of Work pattern for consistency.

## Grounding
- **BPA 4.1 (Double-Entry Axiom)**: Requires that debits and credits always move together as a single atomic unit.
- **Clean Architecture Principles**: Mandates that the Application core is decoupled from Infrastructure implementation details.
