# Copilot Response: GDPR Backlog Gap Analysis (Broad Scope)
**Location:** docs/00_meta/orchestration/responses/w17/02-gdpr-gap-analysis.md

---

## Executive Summary
I merged the two NotebookLM GDPR extractions into one requirement set and compared it against the current backlog. The result is clear: the ledger backlog already covers parts of Article 32 resilience and Article 5 accountability, but it does not yet explicitly cover GDPR privacy-by-design defaults, subject access/portability, or the operational workflow for erasure exclusions and restriction of processing. The architectural choice in [[ADR-001-GDPR-Compliance]] remains valid, but it only solves the redaction mechanism; it does not close the broader GDPR backlog by itself.

## Analysis / Findings
### Merge and de-duplication result
The CSV extraction is the compact seed set. The markdown specification expands the same topic into a broader compliance frame and adds the missing operational rights. There are no hard contradictions between the two sources; the correct merge is to group overlapping statements into the following synthesized requirement clusters:
- Privacy by design and default, including data minimization and pseudonymization.
- Security of processing, including encryption, availability, resilience, and recovery.
- Erasure exclusions and restriction of processing.
- Data access and portability.
- Accountability and records of processing.

### Backlog coverage check
No current user story explicitly names GDPR rights or controls. The existing stories only provide partial support in adjacent areas:
- [[TXEN-002]] normalizes a validated transaction, but it does not enforce data minimization or restrictive privacy defaults.
- [[RECN-001]] and [[OBSV-001]] cover resilience and operational guardrails, but not explicit encryption, recovery evidence, or incident-response requirements.
- [[AUDT-001]] covers auditability and proof-ready finality, but not machine-readable subject-access export or processing-record lineage.

### New requirement clusters
| Synthesized requirement | GDPR reference | Backlog status | Proposed action | MoSCoW priority |
| --- | --- | --- | --- | --- |
| Privacy by design and by default, including data minimization and pseudonymization | Art. 25(1)-(2); Art. 5(1)(c); Art. 32(1)(a) | Missing; only conceptually adjacent to [[TXEN-002]] and [[OBSV-001]] | Add acceptance criteria to [[TXEN-002]] for minimal PII payloads and pseudonymized identifiers; add privacy-default acceptance criteria to [[OBSV-001]] or create a new privacy-guardrail user story | Should |
| Security of processing, including encryption, availability, resilience, and incident recovery | Art. 5(1)(f); Art. 32(1)(a)-(c) | Partially covered by [[OBSV-001]] and [[RECN-001]] | Add acceptance criteria to [[OBSV-001]] for encryption at rest/in transit, restore timing, and incident-response evidence; add recovery-path checks to [[RECN-001]] if reroute security needs to stay explicit | Should |
| Erasure exclusions, legal-hold handling, and restriction of processing | Art. 17(1), Art. 17(3)(b), Art. 18(1) | Missing operational workflow; [[ADR-001-GDPR-Compliance]] only defines the redaction mechanism | Create a new user story for erasure request triage, legal-hold decisioning, restriction flags, and authorized redaction governance | Should |
| Data access and portability | Art. 15(1), Art. 15(3) | Missing | Create a new user story for subject-access and portability export in a commonly used electronic form, with filtering so other subjects' data is not exposed | Should |
| Accountability and records of processing | Art. 5(2); Art. 30(1) | Partially covered by [[AUDT-001]] | Add acceptance criteria to [[AUDT-001]] for machine-readable processing-record evidence and request/response lineage; keep this as a supporting compliance slice | Could |

### What the merge means
The broad markdown extraction adds real scope beyond the CSV. In particular, Article 15 access/export and Article 17.3 erasure exclusions are not represented by the current backlog, and they need separate operational backlog work. The existing story set is strong on auditability and ledger integrity, but it still lacks explicit GDPR rights handling.

## Proposed Plan / Solution
1. Keep [[ADR-001-GDPR-Compliance]] as the architectural enabler for erase-without-fork behavior.
2. Add a GDPR privacy-by-design slice to the backlog by tightening [[TXEN-002]] and [[OBSV-001]].
3. Add a dedicated subject-access / portability story for Article 15.
4. Add a dedicated erasure-exclusion / legal-hold / restriction story for Article 17.3 and Article 18.
5. Add explicit processing-record evidence to [[AUDT-001]] rather than relying on auditability as a proxy for GDPR records.

## Technical Impact
- **Affected Files:** `docs/00_meta/orchestration/responses/w17/02-gdpr-gap-analysis.md`, `docs/00_meta/orchestration/logs/weekly/w17/2026-04-20.md`, `docs/00_meta/orchestration/logs/session-state.md`, `docs/02_analysis/research/w17/High-Integrity Financial Ledger Requirements from GDPR.csv`, `docs/02_analysis/research/w17/Technical-and-Functional-Requirements-Specification_GDPR-Compliance-for High-Integrity-Financial-Ledgers.md`, `docs/02_analysis/requirements/user_stories/TXEN/TXEN-002.md`, `docs/02_analysis/requirements/user_stories/OBSV/OBSV-001.md`, `docs/02_analysis/requirements/user_stories/AUDT/AUDT-001.md`
- **New Dependencies:** None
- **Risk Level:** Medium

## Deliverables / Snippets
### Minimum backlog additions
- Add privacy-by-design acceptance criteria to [[TXEN-002]] and [[OBSV-001]].
- Add security-of-processing acceptance criteria to [[OBSV-001]] and [[RECN-001]].
- Create a new Article 17.3 / Article 18 story for erasure exclusions, legal-hold handling, and restriction of processing.
- Create a new Article 15 story for subject access and portability export.
- Add accountability / processing-record acceptance criteria to [[AUDT-001]].

### ADR-001 impact
[[ADR-001-GDPR-Compliance]] remains the correct answer to the immutability-versus-erasure conflict, but it should be treated as an enabling architecture decision, not as proof of full GDPR compliance. The backlog still needs explicit operational stories for privacy-by-design, access/portability, and legal-hold / restriction workflows.
