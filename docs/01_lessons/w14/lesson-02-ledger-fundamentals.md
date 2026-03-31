---
source: 
  - Fülbier & Sellhorn (2023) 
  - Kahmann et al. (2023) 
  - Wu et al. (2026)
phase: fundamentals
status: draft
last-updated: 2026-03-31
applied-in-project: yes
---

# Lesson 02: Ledger Fundamentals (Event Log vs. World State)

## Objective
Distinguish between the History (the immutable Event Log) and the Current Reality (the World State / materialized view). This distinction is the foundation of Event Sourcing for ledger systems.

## Why It Matters for the Ledger
- **Auditability**: Events record the full narrative of how state changed.
- **Recovery**: Replaying events deterministically rebuilds state after failure.
- **Integrity**: Immutable events provide tamper-evidence and provable history.

## Definitions
- **Event**: an immutable record describing an intent or occurrence (e.g., Deposit, Transfer, Reversal).
- **Projection / World State**: a materialized view derived from applying events in order (e.g., account balances).
- **Aggregate**: a domain entity (e.g., Account) that enforces invariants when processing events.
- **Snapshot**: a periodic capture of World State to speed recovery.
- **Append-only**: events are inserted and not modified (compensating events or redaction protocols are used instead of deletion).

## Key Concepts

### 1. The Append-Only Log (The "Truth")
- Record intent as events rather than updating balances inline.
- Example: instead of `UPDATE account SET balance = 150`, record `Event: Deposit +50`.
- Property: immutability — once appended, an event is part of the official history.

### Event schema (example)
```json
{
  "id": "evt-0001",
  "type": "Transfer",
  "from": "acct:A",
  "to": "acct:B",
  "amount": 10000,
  "currency": "USD",
  "timestamp": "2026-03-31T10:00:00Z",
  "metadata": {"contractId":"C-1234","vendorId":"V-987"},
  "idempotencyKey": "cmd-abc-123"
}
```

### 2. The World State (The "View")
- The World State is derived from the log: InitialState + Σ(events) = CurrentState.
- Materialized views support low-latency queries (e.g., "Does this user have enough funds?").
- Use snapshots to avoid replaying the entire history on recovery.

### 3. Processing Steps (ingest → validate → append → project)
1. **Ingest**: receive command (API), normalize payload, check schema.
2. **Validate**: business rules, balance simulation, anti-fraud checks.
3. **Append**: persist the event atomically to the append-only log (assign sequence).
4. **Project**: apply the event to projections (update balances, journal entries).
5. **Notify / Finalize**: emit finality status, audit proofs, and client response.

- **Idempotency**: include `idempotencyKey` on commands to deduplicate retries.
- **Ordering & Concurrency**: use sequencer/orderer or MVCC and conflict-aware ordering to avoid anomalies.

### 4. Event Sourcing vs. CRUD
- **CRUD** mutates current state directly; **Event Sourcing** records events and derives state.
- Benefits: full audit trail, deterministic rebuild, temporal queries.
- Costs: storage growth, replay complexity, schema evolution overhead.

## Mapping to Double-Entry Accounting
- A `Transfer` event maps to balanced journal postings derived from a single event:
```json
{
  "eventId": "evt-0001",
  "postings": [
    {"account":"acct:A","debit":10000,"credit":0,"currency":"USD"},
    {"account":"acct:B","debit":0,"credit":10000,"currency":"USD"}
  ]
}
```
- The ledger enforces atomicity: postings from one event must preserve Debits = Credits.

## Trade-offs and Mitigations
- **Storage growth**: mitigate with snapshots, compaction, and retention policies.
- **Replay cost**: tune snapshot frequency to meet recovery-time objectives (RTO).
- **Schema evolution**: version event schemas and provide migration tooling.
- **GDPR vs immutability**: prefer reversal/compensating events or plan for redactable-ledger ADRs.

## Common Pitfalls
- Updating state without an event (breaks the source of truth).
- Using floating-point amounts (use integer minor units instead).
- Missing idempotency keys leading to duplicate effects.
- Ignoring partner-bank reconciliation and cross-ledger mapping semantics.

## Mental Model: The Mechatronics Analogy
- The log is the instruction list (G-Code); the world state is machine position.
- After power loss, replay the instruction list or apply a recent snapshot to recover deterministically.

## Applied Example (Transaction Flow)
1. Client submits `Transfer` command with `idempotencyKey`.
2. API validates and simulates the effect against current projection.
3. Event appended with sequence number; sequencer enforces order.
4. Projections update balances and create audit postings that reference the event id.
5. Client receives finality status once committed and projected.

## Operational Notes (short checklist)
- Use integers for monetary amounts; avoid floats.
- Include `idempotencyKey` on all commands.
- Record `contractId`, `vendorId`, and other metadata for auditability.
- Choose snapshot cadence based on TPS and acceptable RTO.

## Interview Notes
- Event Sourcing: immutable events; projections = read models.
- Snapshotting speeds recovery; use reversal events instead of deletion.
- CQRS pairs naturally with Event Sourcing (separate command/write and query/read models).

## Sources
- [[fulbier_2023|Fülbier & Sellhorn, 2023]] — context on double-entry and financial reporting.
- [[kahmann_2023|Kahmann et al., 2023]] — performance contrasts (DAG vs. blockchain).
- [[wu_2026|Wu et al., 2026]] — conflict-aware ordering (cited in meta-analysis).

Note: `wu_2026` bibliographic entry was not found in the repo; please confirm or add its reference file.

## TODO to Internalize
- [ ] Diagram a "Deposit" event vs. a "CRUD Update" on a whiteboard.
- [ ] Research "Snapshotting" best practices for high-throughput ledgers.
- [ ] Explain why deleting a transaction is impossible in a compliant ledger (use a "Reversal Event").
- [ ] Practice mapping a `Transfer` event into journal postings and audit proofs.
