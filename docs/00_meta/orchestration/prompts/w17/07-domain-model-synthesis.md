---
status: completed
issued_by: Gemini CLI
issued_date: 2026-04-24
response_ref: docs/00_meta/orchestration/responses/w17/07-domain-model-synthesis.md
---
# Task: Canonical Domain Model Synthesis (Gemini + Claude + Antigravity)

## Task Context
We are finalizing the Domain Entity Model before moving to the implementation phase. We have multiple inputs: the Gemini HLD/ADR stack and a rich draft from Claude that includes invariants and ISO-specific validation sequences. We need a single, high-integrity canonical model.

## Objectives
1.  **Synthesize**: Overwrite `docs/03_architecture/Domain-Entity-Model.md` using the "Richness" of the Claude draft (Invariants, OCL rules, 8-level validation) and the "System Continuity" of the Gemini ADRs.
2.  **Critically Expand**: Identify and resolve missing architectural details required for a distributed B2B ledger.
3.  **Archive**: After the synthesis is complete and verified, move the source draft files (`docs/03_architecture/Domain-Entity-Model-claude.md` and `docs/03_architecture/Domain-Entity-Model-antigravity.md`) to `docs/00_meta/archive/architecture_drafts/`.

## Source Materials
- `docs/03_architecture/Domain-Entity-Model-claude.md` (Rich metadata and diagrams)
- `docs/03_architecture/Domain-Entity-Model-antigravity.md` (Comparative draft)
- `docs/03_architecture/High-Level-Design.md` (Service boundaries)
- `docs/03_architecture/adr/ADR-003-Sharding-Topology-via-MSSP.md` (m-node ratio)

## Specific Synthesis Instructions
1.  **Invariants**: Adopt the "Legal metadata as a consensus predicate" invariant.
2.  **Validation**: Include the 8-Level Gateway Validation checklist.
3.  **Sharding Topology**: Ensure the model reflects the **20%-25% m-node ratio**. Explicitly add a `ShardID` or `ConsensusZoneID` to relevant entities.
4.  **Audit Layer**: The `AuditBlock` must carry the `ChameleonHash` (ADR-001) and `QuorumCert` (ADR-002).

## Critical Analysis (Copilot self-correction)
Please be critical of the current model and add the following missing "High-Integrity" details:
- **Shard Partitioning Logic**: Define the attribute used as the "Partition Key" (e.g., `OwnerLEI` or `AccountID`) to ensure deterministic routing to shards.
- **Timezone Precision**: Enforce UTC 'Z' (ISO 8601) across all `DateTimeOffset(7)` fields to prevent cross-node clock drift.
- **State Versioning**: Add a `Version` or `SequenceNumber` to the `Balance` projection to prevent "Stale Read" issues during high-frequency throughput.
- **Error Handling**: Define a standard `RejectionRecord` or `NegativeAcknowledgement` (pacs.002) mapping.

## Expected Output
1. A comprehensive, canonical `Domain-Entity-Model.md`.
2. The archival of draft files to `docs/00_meta/archive/architecture_drafts/`.
