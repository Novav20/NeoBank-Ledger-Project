---
Status: Proposed
Date: 2026-04-20
Deciders: Gemini CLI / Juan David
Supersedes: None
---

# ADR-001: GDPR Compliance via Redactable Ledger

## Context
BPA v1.2 section 4.1 requires immutable history, cryptographic provenance, and explicit finality for the B2B ledger core. At the same time, GDPR Article 17 requires the system to support erasure of personal data when legally justified. A pure append-only ledger cannot satisfy both requirements for PII carried directly on-chain without an additional redaction mechanism.

## Decision
We will use a redactable ledger architecture based on chameleon hash functions so authorized redaction of PII can occur without breaking the hash chain or forcing a hard fork. Redaction events will be governed by strict trapdoor-key controls and audit logging.

## Consequences

### Positive
- Supports GDPR Article 17 erasure workflows for PII while preserving ledger structure.
- Preserves hash-chain continuity and auditability.
- Enables correction of fraudulent entries without a hard fork.

### Negative / Trade-offs
- Increases cryptographic complexity in the ledger core.
- Introduces operational overhead for redaction governance, review, and audit.
- Requires explicit trapdoor-key custody and recovery procedures.

### Risks
- Trapdoor-key compromise could enable unauthorized redaction; mitigate with HSM-backed custody, dual control, separation of duties, and immutable audit logging.

## Alternatives Considered
| Alternative | Reason Rejected |
|---|---|
| Key Shredding | Leaves encrypted artifacts in history and may not satisfy strict future audit expectations. |
| Hard Forks | Too disruptive for a B2B ledger and breaks continuity across participants. |

## References
- [[BPA_Report#4.1 Foundational Principles (Core Business Rules)|BPA Report v1.2, Section 4.1 Foundational Principles]]
- GDPR Regulation (EU) 2016/679, Article 17 (Right to Erasure)
- [[zhao_2024|Zhao et al. (2024)]], "Concordit: A credit-based incentive mechanism for permissioned redactable blockchain"
