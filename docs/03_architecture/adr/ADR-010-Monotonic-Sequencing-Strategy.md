---
Status: Proposed
Date: 2026-05-06
Deciders: Gemini CLI / Juan David
Supersedes: None
---

# ADR-010: Monotonic Sequencing Strategy

## Context
The Ledger follows an Event Sourcing pattern (BPA 5.2) where the "Truth" is derived from an ordered log of events. RocksDB, our chosen persistence engine, is a Key-Value store and does not provide native auto-incrementing sequences or identity columns. We need a strategy to generate and persist a global, monotonic, gap-less sequence number for every event to ensure deterministic replay and ledger integrity.

## Decision
We will implement a **Metadata-Key-backed Monotonic Sequencer** within the Infrastructure layer.

### 1. Persistence Strategy
- We will reserve a specific key: `meta:last_sequence`.
- This key will store a `long` value representing the highest sequence number successfully committed to the store.

### 2. Atomic Increment Logic
- Upon system initialization, the `RocksDbStore` will load the current value from `meta:last_sequence` into an in-memory counter.
- For every new event:
    1. The counter is incremented in a thread-safe manner (using `Interlocked.Increment`).
    2. The incremented value is assigned to the Event entity.
    3. The new Event and the updated `meta:last_sequence` value are committed to RocksDB as a single **Atomic WriteBatch**.

### 3. Key Formatting
- Event keys will maintain the ADR-008 format: `evt:{SequenceNumber:D20}`.
- Zero-padding to 20 digits ensures that lexicographical sorting in RocksDB matches the numerical sequence.

## Consequences

### Positive
- **Guaranteed Order**: Ensures that events are always stored and retrieved in the order they were validated.
- **Resilience**: Storing the counter in the same WriteBatch as the event prevents "Phantom Sequences" (where a counter increments but the data save fails).
- **Performance**: Reading from a specific metadata key is faster than performing a `Last()` scan on the entire event namespace.

### Negative
- **Single-Node Limitation**: A single global counter is a bottleneck for multi-node writes. 
- **Mitigation**: For the MVP, the Sequencer is a single-node logical role (as per HLD Step 2). Scaling will be addressed in Sprint 03 via Sharding (ADR-003).

## Grounding
- **BPA 5.2 (Ordering Service)**: Explicitly defines the requirement for a global sequence number before validation.
- **ISO 20022**: Supports the need for deterministic delivery order and non-repudiation.
