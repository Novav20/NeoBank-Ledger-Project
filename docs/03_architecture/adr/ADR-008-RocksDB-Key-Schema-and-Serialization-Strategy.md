---
Status: Proposed
Date: 2026-05-04
Deciders: Gemini CLI / Juan David
Supersedes: "[[ADR-006-LevelDB-Key-Schema-and-Serialization-Strategy]]"
---

# ADR-008: RocksDB Key Schema and Serialization Strategy

## Context
The Ledger requires a high-performance, append-only persistence layer for the Event Store and World State (Balances). Originally, LevelDB was proposed (ADR-006). However, subsequent analysis of the modern .NET ecosystem reveals that RocksDB (a fork of LevelDB optimized for SSDs and multi-core environments) provides vastly superior performance, particularly regarding multi-threaded compaction and concurrent reads/writes. Modern wrappers like `RocksDb.Net` also leverage C# `Span<byte>` for zero-allocation memory access. We still require a deterministic strategy to isolate entity types and ensure lexicographical ordering for sequential event replay.

## Decision
We will adopt **RocksDB** as the storage engine using modern .NET wrappers. We will maintain the **Namespaced Key Strategy** and **JSON Serialization** originally established in ADR-006.

### 1. Key Naming Convention (Namespacing)
We will use string prefixes followed by a delimiter (`:`) to simulate tables.

| Entity | Key Pattern | Example |
| :--- | :--- | :--- |
| **Account** | `acc:{Guid}` | `acc:550e8400-e29b-41d4-a716-446655440000` |
| **Event** | `evt:{SequenceNumber:D20}` | `evt:00000000000000000001` |
| **Balance** | `bal:{Guid}` | `bal:550e8400-e29b-41d4-a716-446655440000` |
| **Party** | `pty:{Guid}` | `pty:550e8400-e29b-41d4-a716-446655440000` |
| **AuditBlock** | `aud:{BlockHeight:D20}`| `aud:00000000000000000001` |
| **RejectionRecord**| `rej:{UTI}` | `rej:UTI123456789...` |

*Note: Sequence numbers for Events and BlockHeights for AuditBlocks will be zero-padded to 20 digits to ensure correct alphabetical sorting in RocksDB SSTables.*

### 2. Serialization Format
We will use **System.Text.Json** for all serialization.
- **Why?**: It is native to .NET 10, high-performance, and human-readable for debugging.
- **Policy**: We will always map Domain Entities to **Persistence DTOs** before serialization.

### 3. Delimiter Selection
The colon (`:`) is selected as the standard delimiter, following common Key-Value store practices.

## Consequences
- **Positive**: RocksDB eliminates "write stalls" present in LevelDB via multi-threaded compaction; high-performance range scans; modern .NET memory efficiencies.
- **Negative**: Native C++ dependency management is required (though handled seamlessly by modern NuGet packages).
- **Mitigation**: We will use standard `RocksDb.Net` to abstract the native interop complexities.

## Grounding
- **BPA 5.2**: Supports the Event Sourcing pattern requiring sequential replay at high throughput.
