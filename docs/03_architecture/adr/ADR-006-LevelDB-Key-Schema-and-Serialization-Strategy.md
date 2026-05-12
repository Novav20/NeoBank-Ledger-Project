---
Status:
  - Proposed
Date: 2026-04-29
Deciders: Gemini CLI / Juan David
Supersedes: None
---

# ADR-006: LevelDB Key Schema and Serialization Strategy

## Status
Superseded by ADR-008

## Context
The Ledger requires a high-performance, append-only persistence layer for the Event Store and World State (Balances). We have selected LevelDB as the storage engine (Phase D). LevelDB is a flat Key-Value store with no built-in schema or table support. We need a deterministic strategy to:
1. Distinguish between different entity types (Accounts, Events, Balances).
2. Ensure correct lexicographical ordering for range scans (e.g., replaying events in order).
3. Serialize Domain Entities into a format suitable for disk storage.

## Decision
We will adopt a **Namespaced Key Strategy** and **JSON Serialization** for all persistence operations within LevelDB.

### 1. Key Naming Convention (Namespacing)
We will use string prefixes followed by a delimiter (`:`) to simulate tables.

| Entity | Key Pattern | Example |
| :--- | :--- | :--- |
| **Account** | `acc:{Guid}` | `acc:550e8400-e29b-41d4-a716-446655440000` |
| **Event** | `evt:{SequenceNumber:D20}` | `evt:00000000000000000001` |
| **Balance** | `bal:{Guid}` | `bal:550e8400-e29b-41d4-a716-446655440000` |

*Note: Sequence numbers for Events will be zero-padded to 20 digits to ensure correct alphabetical sorting in LevelDB SSTables.*

### 2. Serialization Format
We will use **System.Text.Json** for all serialization.
- **Why?**: It is native to .NET 10, high-performance, and human-readable for debugging the underlying LevelDB files.
- **Policy**: We will always map Domain Entities to **Persistence DTOs** before serialization to decouple the business logic from the storage format.

### 3. Delimiter Selection
The colon (`:`) is selected as the standard delimiter, following common NoSQL practices (Redis/RocksDB).

## Consequences
- **Positive**: High-performance range scans for event replay; clear isolation between data types; human-readable storage for auditing.
- **Negative**: String-based keys add a small overhead compared to raw binary keys; JSON is more verbose than binary formats like Protobuf.
- **Mitigation**: LevelDB's internal compression will minimize the overhead of string prefixes and JSON verbosity.

## Grounding
- **BPA 5.2**: Supports the Event Sourcing pattern requiring sequential replay.
- **Industry Practice**: Adheres to the Namespacing pattern seen in high-throughput KV stores.
