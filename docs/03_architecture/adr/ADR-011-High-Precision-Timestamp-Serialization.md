---
Status: Proposed
Date: 2026-05-14
Deciders: Gemini CLI / Juan David
Supersedes: None
---

# ADR-011: High-Precision Timestamp Serialization

## Context
MiFID II (RTS 25) requires high-integrity financial systems to record event timestamps with at least 1-millisecond (1ms) precision. We are using RocksDB with JSON serialization (ADR-008). If the serialization process rounds or truncates the fractional seconds of a `DateTimeOffset`, the system will fail to meet regulatory standards and may lose deterministic ordering during replay.

## Decision
We will use **ISO 8601 UTC strings with 7 decimal places of precision** for all persisted timestamps.

### 1. Storage Format
- All timestamps MUST be converted to **UTC (Z)** before serialization.
- The format will follow the standard ISO 8601 pattern: `YYYY-MM-DDTHH:MM:SS.fffffffZ`.
- This ensures that 100-nanosecond (tick-level) precision is preserved on disk, which far exceeds the 1ms MiFID II requirement.

### 2. Implementation
- We will leverage the default behavior of `System.Text.Json` in .NET 10, which handles `DateTimeOffset` serialization using this standard format.
- We will NOT use custom "Unix Ticks" or "Long" formats unless performance testing identifies JSON string parsing as a bottleneck.

## Consequences

### Positive
- **Regulatory Compliance**: Exceeds the 1ms precision required by MiFID II RTS 25.
- **Human Readability**: Timestamps in the raw RocksDB files are easily readable by auditors and developers.
- **Interoperability**: ISO 8601 is the universal standard for date-time strings across all banking systems.

### Negative
- **Storage Size**: String-based timestamps take more bytes than raw Int64 ticks.
- **Mitigation**: RocksDB's internal compression effectively reduces the storage overhead of repetitive ISO string prefixes.

## Grounding
- **MiFID II RTS 25**: Mandates 1ms clock synchronization and recording precision for electronic trading.
- **ISO 20022**: Supports UTC-based ISO 8601 time representations for message traceability.
