---
type: user-story
version: 1.0
last_edited: 2026-04-20
status: draft
story_id: AUDT-003
epic_id: AUDT
epic_name: Integrity & Audit Vault
title: Enforce erasure exclusions and legal-hold restrictions
persona: Sponsor Bank
goal: block redaction when data must be retained for audit, legal claims, or statutory recordkeeping
business_value: prevent unlawful erasure and keep redaction decisions aligned with legal retention obligations
acceptance_format: gherkin-table
source:
  - docs/02_analysis/requirements/epics.md#AUDT
  - docs/02_analysis/research/w17/Technical-and-Functional-Requirements-Specification_GDPR-Compliance-for High-Integrity-Financial-Ledgers.md
  - docs/02_analysis/research/w17/High-Integrity Financial Ledger Requirements from GDPR.csv
  - docs/03_architecture/adr/ADR-001-GDPR-Compliance.md
derived_from:
  - docs/02_analysis/requirements/epics.md#AUDT
related_use_cases:
  - UC-03
related_fr: []
related_nfr: []
depends_on:
  - "[[ADR-001-GDPR-Compliance]]"
  - "[[AUDT-001]]"
priority: Should
estimate: 5
verification:
  - acceptance test
  - compliance review
risk_level: Critical
notes:
  - Rationale: GDPR Articles 17(3)(b) and 18(1) allow retention or restriction when legal obligations or claims require continued processing.
  - ADR-001 provides the redaction mechanism, but this story governs when redaction must be blocked.
---

# AUDT-003: Enforce Erasure Exclusions and Legal-Hold Restrictions

## Story Statement
As a Sponsor Bank, I want redaction to be blocked when data must be retained for audit or legal claims, so that erasure requests do not violate statutory retention duties.

## Context
This story covers GDPR erasure exclusions and restriction of processing within the audit vault epic. It sits on top of the redactable-ledger mechanism from ADR-001 and ensures the system can refuse or defer redaction when legal-hold conditions apply.

## Acceptance Criteria
Use a Gherkin-inspired table. Keep one row per atomic criterion.

| AC ID | Given | When | Then | Notes |
| --- | --- | --- | --- | --- |
| AC-001 | A redaction request targets data under legal hold | The request is evaluated | The system blocks the redaction and records the retention reason | |
| AC-002 | A record is needed for financial audit or legal claims | Restriction of processing is applied | The system marks the record as restricted instead of deleting it | |
| AC-003 | A legal hold is lifted | The request is re-evaluated | Authorized redaction can proceed through the ADR-001 redactable-ledger mechanism | |

## Dependencies
- [[ADR-001-GDPR-Compliance]]
- [[AUDT-001]]

## Notes
- Rationale: GDPR Article 17(3)(b) exempts records needed for legal obligations or claims, and Article 18(1) requires restriction rather than deletion when processing must be paused.
- Redaction requests must be auditable, and any blocked request should remain visible in the compliance trail.
