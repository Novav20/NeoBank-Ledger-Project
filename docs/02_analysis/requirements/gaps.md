---
type: gap-note
status: open
related: [../bpa/BPA_Report.md, ./use_cases/specifications.md, ./epics.md, ./user_stories.base, ./functional_requirements, ./non_functional_requirements]
focus: requirements-traceability
---

# Requirements Gap Register

This file records ambiguities and closed traceability gaps across requirements artifacts. Unsupported behavior must stay here until it is confirmed by source material.

## Open Gaps

| Gap | Why it matters | Current status |
| --- | --- | --- |
| Validation shard failure handling | BPA 5.2 defines validation, but it does not specify the failover, retry, or rejection behavior when a validation shard is unavailable. | Open |
| Health monitoring scope | UC-05 needs a precise definition of which metrics are mandatory, which thresholds are enforced, and which alerts are emitted. | Open |
| Reconciliation trigger and correction path | BPA describes external liquidity reconciliation, but it does not say whether the process is scheduled, event-driven, or tied to an exception workflow. | Open |
| BPA availability threshold provenance | BPA NFR-AVAIL-01 sets view-change <30s, but the reviewed source summaries only explicitly support BFT quorum and view-change behavior, not the exact recovery bound. | Open |
| RocksDB snapshot and backup strategy | The current persistence implementation proves append, replay, and validation behavior, but it does not yet define how snapshots, backups, or restore workflows should be scheduled and verified. | Open |

## Closed Gaps

| Gap | Why it mattered | Why it was a gap | Resolution | Evidence basis | Current status |
| --- | --- | --- | --- | --- | --- |
| Audit proof delivery mode | BPA supports audit proofs and checkpoint evidence, but it did not fully define whether verification was pull-based, push-based, or both. | The gap existed because the evidence delivery model needed to be explicit before FR-005 could be written. | Hybrid model: shards periodically publish signed hash-chain checkpoints (Merkle roots) to an evidence repository for push-based oversight, while individual transaction proofs are served via API on demand for pull-based verification. | BPA evidence on signed hash-chain checkpoints and audit trails ([[boukhatmi_2025]]; [[mashiko_2025]]; [[sonnino_2021]]) | Closed |
| Regulatory access scope in UC-03 | The BPA supports auditors and regulators, but the exact distinction between active verification and passive evidence consumption was not explicit enough for a final BPMN boundary. | The gap existed because UC-03 needed a precise boundary before the audit story could be derived. | Passive oversight: regulators use observer nodes to post-validate published checkpoints without participating in consensus or settlement. | BPA regulators/auditors guidance and observer-node research ([[boukhatmi_2025]]; [[mashiko_2025]]; [[sonnino_2021]]) | Closed |
| Traceability granularity for NFRs | Story metadata already references NFR identifiers, but the BPA NFR registry needed dedicated files before those links became concrete file targets. | The gap existed while the BPA NFR registry was still being split into individual NFR files. | All BPA NFR rows now exist as dedicated files, so story metadata can link to concrete NFR targets directly. | BPA NFR registry split across `NFR-001.md` through `NFR-016.md` | Closed |

## Sprint 01 Validation Findings

- Atomicity is verified by the real RocksDB-backed `ILedgerUnitOfWork` integration test.
- Immutability is verified by the `EventCollisionException` guard path in the RocksDB event store.
- 1ms precision and UTC preservation are verified by the precision validation test suite.
- The persistence layer is now regulatory-ready for MiFID II validation coverage.

## Research Notes Used During Extraction
- Research on observer and monitor nodes supports the existence of operational health checking, but it does not define the exact monitoring thresholds.
- Research on sharded BFT and view-change behavior supports availability assumptions, but not the exact response to a failed validation shard in this project.
- Research on availability supports BFT quorum and view-change behavior, but the exact <30s recovery bound remains a BPA synthesis point rather than a source-level statement in the reviewed summaries.