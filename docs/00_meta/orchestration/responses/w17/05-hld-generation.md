# Copilot Response: High-Level Design (HLD) Generation
**Location:** docs/00_meta/orchestration/responses/w17/05-hld-generation.md

---

## Executive Summary
I created the High-Level Design as a service-level map from transaction intake through sequencing, shard-group consensus, cross-shard bridging, and audit logging. The HLD is grounded in BPA Section 5.2 and applies the completed ADR stack: ADR-001 for redactable shard storage, ADR-002 for PBFT finality, and ADR-003 for the m-node shard topology.

## Analysis / Findings
The HLD makes the architecture traceable end to end without dropping into implementation detail. The main flow is:
- Fintech Partner -> Gateway for auth and normalization.
- Gateway -> Sequencer for global sequence assignment.
- Sequencer -> Shard-Groups for PBFT validation.
- m-nodes bridge the shard-groups for cross-shard atomicity.
- Accepted shard outputs flow into the Audit Vault, while the Balance Projection is updated as a byproduct of validation.

The design keeps the world state as a materialized view rather than a competing system of record. That separation matters because it keeps the event log authoritative while still serving low-latency balance queries.

## Proposed Plan / Solution
1. Treat the HLD as the canonical map for the architecture baseline.
2. Use the HLD to drive the next step: formal domain modeling.
3. Keep the remaining design work at the service boundary level until the entity schema phase begins.

## Technical Impact
- **Affected Files:** `docs/03_architecture/High-Level-Design.md`, `docs/00_meta/orchestration/logs/weekly/w17/2026-04-21.md`, `docs/00_meta/orchestration/logs/session-state.md`, `docs/00_meta/plans/w17.md`
- **New Dependencies:** None
- **Risk Level:** Medium

## Deliverables / Snippets
### HLD flow
- Intake: Fintech Partner -> Gateway
- Sequencing: Gateway -> Ordering Service / Sequencer
- Validation: Sequencer -> Shard-Groups
- Cross-shard bridge: m-nodes with a 20%-25% ratio
- Settlement and audit: Shards -> Audit Vault -> Observer

### ADR integration
- ADR-001: chameleon-hash-backed shard storage
- ADR-002: PBFT with $2f+1$ quorum per shard-group
- ADR-003: 20%-25% m-node ratio for shard topology
