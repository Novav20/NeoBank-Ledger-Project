# Copilot Response: GDPR Backlog Expansion
**Location:** docs/00_meta/orchestration/responses/w17/03-gdpr-backlog-expansion.md

---

## Executive Summary
I expanded the backlog to cover the GDPR gaps identified in the broad analysis. The existing stories now carry explicit privacy-by-design, security, and processing-lineage controls, and two new AUDT stories cover subject access/portability and erasure exclusions/legal hold. ADR-001 remains the enabling architecture for redactable history, but the compliance story is now represented in the backlog rather than only in the architecture decision.

## Analysis / Findings
### Existing story updates
- [[TXEN-002]] now explicitly covers data minimization and pseudonymized identifiers during transaction normalization.
- [[OBSV-001]] now explicitly covers privacy-by-default access restrictions and encryption at rest/in transit.
- [[AUDT-001]] now explicitly covers machine-readable processing-record lineage.

### New backlog items
| Story | GDPR coverage | Priority | Purpose |
| --- | --- | --- | --- |
| [[AUDT-002]] | Articles 15(1), 15(3), 20 | Should | Subject access and portability export in a machine-readable form without leaking other subjects' data. |
| [[AUDT-003]] | Articles 17(3)(b), 18(1) | Should | Block redaction when retention or legal-hold obligations require continued processing. |

### Why this is the right expansion
The broad GDPR extraction shows that ledger integrity is necessary but not sufficient. A redactable ledger solves only the hard technical conflict between immutability and erasure. The backlog also needs explicit operational slices for privacy-by-design defaults, DSAR export, and legal-hold handling so the system can demonstrate GDPR compliance in practice rather than only in principle.

## Proposed Plan / Solution
1. Keep the updated TXEN, OBSV, and AUDT stories as the GDPR-backed baseline.
2. Treat [[AUDT-002]] and [[AUDT-003]] as Should items so the MVP scope remains centered on the core ledger trust path.
3. Use [[ADR-001-GDPR-Compliance]] as the redaction mechanism and compliance enabler for the erasure workflow.
4. Carry the new GDPR stories into the next planning pass when backlog ordering is revisited.

## Technical Impact
- **Affected Files:** `docs/02_analysis/requirements/user_stories/TXEN/TXEN-002.md`, `docs/02_analysis/requirements/user_stories/OBSV/OBSV-001.md`, `docs/02_analysis/requirements/user_stories/AUDT/AUDT-001.md`, `docs/02_analysis/requirements/user_stories/AUDT/AUDT-002.md`, `docs/02_analysis/requirements/user_stories/AUDT/AUDT-003.md`, `docs/00_meta/orchestration/logs/weekly/w17/2026-04-20.md`, `docs/00_meta/orchestration/logs/session-state.md`
- **New Dependencies:** None
- **Risk Level:** Medium

## Deliverables / Snippets
### Backlog changes
- Added AC-004 and AC-005 to [[TXEN-002]] for data minimization and pseudonymization.
- Added AC-007 and AC-008 to [[OBSV-001]] for privacy-by-default and encryption.
- Added AC-006 to [[AUDT-001]] for processing-record lineage.
- Created [[AUDT-002]] for subject access and portability.
- Created [[AUDT-003]] for erasure exclusions and legal-hold restrictions.

### Compliance result
The backlog now explicitly represents the GDPR concerns raised in the gap analysis. ADR-001 still handles the immutable-history/redaction mechanism, but the ledger now also has concrete user stories for the operational rights and controls that surround it.
