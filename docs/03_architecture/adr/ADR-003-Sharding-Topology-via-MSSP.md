---
Status: Proposed
Date: 2026-04-21
Deciders: Gemini CLI / Juan David
Supersedes: None
---

# ADR-003: Sharding Topology via MSSP

## Context
The BPA architecture requires a scalable execution and validation model without sacrificing ledger integrity. Cross-shard transactions become unnecessarily complex when the topology requires heavyweight coordination such as 2PC for every boundary crossing, especially in a high-transaction B2B ledger.

## Decision
We will adopt a Multi-Shard Storage Protocol (MSSP) topology with an m-node ratio of 20%-25%. This allows m-nodes to participate in multiple consensus zones and reduces the need for Two-Phase Commit across shards.

## Consequences

### Positive
- Linear throughput scaling as shard capacity grows.
- Reduced confirmation latency for cross-shard flows.
- Better alignment with the project’s high-transaction throughput targets.

### Negative / Trade-offs
- Higher storage and bandwidth requirements for m-nodes.
- More complex node provisioning and capacity planning.

### Risks
- An undersized m-node pool could create bottlenecks for cross-shard coordination; mitigate by enforcing the 20%-25% ratio during capacity planning and validating it against deployment size.

## Alternatives Considered
| Alternative | Reason Rejected |
|---|---|
| Two-Phase Commit (2PC) | Too complex and coordination-heavy for cross-shard transactions at ledger scale. |
| Uniform shard nodes without m-nodes | Would increase cross-shard coordination cost and reduce the efficiency of conflict-aware execution. |

## References
- [[BPA_Report#4.1 Foundational Principles (Core Business Rules)|BPA 4.1 Foundational Principles]]
- [[BPA_Report#5.2 To-Be Flow (Event Sourcing + Shards + Fabric-like EOV)|BPA 5.2 To-Be Flow]]
- [[liu_2025|Liu et al. (2025)]]
- [[yu_2023_batch03|Yu et al. (2023)]]
- [[wang_2026|Wang et al. (2026)]]
