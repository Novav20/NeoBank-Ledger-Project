---
type: gap-note
status: open
related: [../bpa/BPA_Report.md, ../bpmn/actors-stakeholders.md, ../bpmn/processes.md, ./specifications.md]
focus: use-case-extraction
---

# Use Case Extraction Gaps

This file records ambiguities that remain after reviewing the BPA report, BPMN extraction notes, and supporting research artifacts. Unsupported behavior must stay here until it is confirmed by source material.

## Open Gaps

| Gap | Why it matters | Current status |
| --- | --- | --- |
| Validation shard failure handling | BPA 5.2 defines validation, but it does not specify the failover, retry, or rejection behavior when a validation shard is unavailable. | Open |
| Health monitoring scope | UC-05 needs a precise definition of which metrics are mandatory, which thresholds are enforced, and which alerts are emitted. | Open |
| Reconciliation trigger and correction path | BPA describes external liquidity reconciliation, but it does not say whether the process is scheduled, event-driven, or tied to an exception workflow. | Open |
| Audit proof delivery mode | BPA supports audit proofs and checkpoint evidence, but it does not fully define whether verification is pull-based, push-based, or both. | Open |
| Regulatory access scope in UC-03 | The BPA supports auditors and regulators, but the exact distinction between active verification and passive evidence consumption is still not explicit enough for a final BPMN boundary. | Open |

## Resolved Gaps

| Gap | Resolution | Evidence basis |
| --- | --- | --- |
| Audit proof delivery mode | Hybrid model: shards periodically publish signed hash-chain checkpoints (Merkle roots) to an evidence repository for push-based oversight, while individual transaction proofs are served via API on demand for pull-based verification. | BPA evidence on signed hash-chain checkpoints and audit trails ([[boukhatmi_2025]]; [[mashiko_2025]]; [[sonnino_2021]]) |
| Regulatory access scope in UC-03 | Passive oversight: regulators use observer nodes to post-validate published checkpoints without participating in consensus or settlement. | BPA regulators/auditors guidance and observer-node research ([[boukhatmi_2025]]; [[mashiko_2025]]; [[sonnino_2021]]) |

## Research Notes Used During Extraction
- Research on observer and monitor nodes supports the existence of operational health checking, but it does not define the exact monitoring thresholds.
- Research on sharded BFT and view-change behavior supports availability assumptions, but not the exact response to a failed validation shard in this project.
