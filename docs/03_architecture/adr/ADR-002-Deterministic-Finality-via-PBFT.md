---
Status: Proposed
Date: 2026-04-21
Deciders: Gemini CLI / Juan David
Supersedes: None
---

# ADR-002: Deterministic Finality via PBFT

## Context
BPA v1.2 requires explicit finality for the B2B ledger core, and the foundational principles in section 4.1 state that deterministic finality is required for trust-sensitive settlement. A consensus model that permits unresolved fork risk would weaken the auditability and non-repudiation guarantees expected by financial partners.

## Decision
We will use PBFT for the core consensus cluster to provide immediate deterministic finality for ledger writes. RAFT is rejected for the core trust path because it provides crash-fault tolerance only and does not satisfy the Byzantine threat model.

## Consequences

### Positive
- Immediate deterministic finality for trusted B2B settlement.
- Mature ecosystem support, including BFT-SMaRt-style implementations.
- Clear fork-freedom semantics for audit and partner assurance.

### Negative / Trade-offs
- Communication overhead grows quadratically with cluster size.
- Operational complexity is higher than CFT-only consensus models.

### Risks
- PBFT message overhead can become a performance bottleneck if the cluster is oversized; mitigate by keeping the core cluster bounded and using the sharded topology defined in ADR-003.

## Alternatives Considered
| Alternative | Reason Rejected |
|---|---|
| RAFT | CFT-only consensus is insufficient for trust-sensitive partners and does not deliver Byzantine fault tolerance. |
| HotStuff | Strong candidate for BFT finality, but PBFT is selected here as the baseline due to the explicit project research and library alignment. |

## References
- [[BPA_Report#4.1 Foundational Principles (Core Business Rules)|BPA 4.1 Foundational Principles]]
- [[BPA_Report#5.2 To-Be Flow (Event Sourcing + Shards + Fabric-like EOV)|BPA 5.2 To-Be Flow]]
- [[benedetti_2022|Benedetti et al. (2022)]]
- [[barger_2021|Barger et al. (2021)]]
- [[chan_2018|Chan et al. (2018)]]
