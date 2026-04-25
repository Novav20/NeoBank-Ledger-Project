## Executive Summary
The canonical domain model was rewritten to merge the Gemini HLD/ADR spine with the useful richness from the Claude and antigravity drafts. The result keeps the ledger pre-code and implementation-ready while adding explicit shard routing, validation states, ordered event flow, and rejection handling.

## Analysis / Findings
- The previous canonical model was too lean for a distributed B2B ledger because it did not surface the validation ladder, routing key, or failure path explicitly.
- The Claude draft contributed the useful architectural richness: consensus predicates, validation progression, target-market controls, and stale-read protection.
- The antigravity draft contributed a useful reminder that the model must preserve the log/projection distinction, but its naming was too abstract for the final canonical file.
- The final canonical document now includes Party, Transaction, Event, Entry, Balance, AuditBlock, and RejectionRecord, with `ShardId` and `ConsensusZoneId` carried through the routing-sensitive entities.
- Balance remains a projection with versioning, and AuditBlock remains the immutable evidence anchor with `ChameleonHash` and `QuorumCert`.

## Proposed Plan / Solution
1. Treat `docs/03_architecture/Domain-Entity-Model.md` as the single source of truth.
2. Keep the eight-state gateway validation sequence as the ingress contract.
3. Use `OwnerLEI` as the deterministic routing key for customer-owned flows, with `AccountId` reserved for internal or house accounts.
4. Preserve `BIGINT` minor-unit storage and UTC `DateTimeOffset(7)` timestamps across the physical model.
5. Archive non-canonical draft inputs so future work does not accidentally cite them as the active design.

## Technical Impact
- **Affected Files:**
  - `docs/03_architecture/Domain-Entity-Model.md`
  - `docs/00_meta/archive/architecture_drafts/Domain-Entity-Model-claude.md`
  - `docs/00_meta/archive/architecture_drafts/Domain-Entity-Model-antigravity.md`
- **New Dependencies:** None
- **Risk Level:** Low

## Deliverables / Snippets
- Canonical domain model updated with validation, routing, event sequencing, and rejection semantics.
- Draft sources archived under `docs/00_meta/archive/architecture_drafts/`.
- The canonical model now reflects the HLD, ADR-001, ADR-002, and ADR-003 without keeping the draft variants in the active architecture path.
- Controlled-vocabulary fields were tightened with explicit allowed-value constraints, including `PartyRole` and lifecycle/status columns.
