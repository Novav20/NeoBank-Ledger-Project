---
type: user-story
version: 1.0
last_edited: 2026-04-20
status: draft
story_id: AUDT-002
epic_id: AUDT
epic_name: Integrity & Audit Vault
title: Provide subject access and data portability exports
persona: Sponsor Bank
goal: export a data subject's personal data in a commonly used electronic form without exposing other subjects' information
business_value: satisfy GDPR access and portability rights while preserving audit boundaries and minimizing disclosure risk
acceptance_format: gherkin-table
source:
  - docs/02_analysis/requirements/epics.md#AUDT
  - docs/02_analysis/research/w17/Technical-and-Functional-Requirements-Specification_GDPR-Compliance-for High-Integrity-Financial-Ledgers.md
  - docs/02_analysis/research/w17/High-Integrity Financial Ledger Requirements from GDPR.csv
derived_from:
  - docs/02_analysis/requirements/epics.md#AUDT
related_use_cases:
  - UC-03
related_fr: []
related_nfr: []
depends_on:
  - "[[AUDT-001]]"
implementation_path: src/NeoBank.Ledger.Application/
test_path: tests/NeoBank.Ledger.Infrastructure.Tests/
sprint: Sprint 01
priority: Should
estimate: 5
verification:
  - acceptance test
  - export validation
risk_level: High
notes:
  - Rationale: GDPR Articles 15(1), 15(3), and 20 require access and portability in a commonly used electronic form.
  - Exports must exclude other data subjects' data and preserve the integrity of the audit trail.
---

# AUDT-002: Provide Subject Access and Data Portability Exports

## Story Statement
As a Sponsor Bank, I want to export a data subject's personal data in a commonly used electronic form, so that I can satisfy access and portability requests without leaking other subjects' information.

## Context
This story covers subject access and portability within the audit vault epic. It reuses the ledger's historical evidence while constraining the export boundary to the requesting subject.

## Acceptance Criteria
Use a Gherkin-inspired table. Keep one row per atomic criterion.

| AC ID | Given | When | Then | Notes |
| --- | --- | --- | --- | --- |
| AC-001 | A data subject access request is received | The export is generated | The system returns the requesting subject's personal data in a commonly used electronic format | |
| AC-002 | The request includes transaction history and metadata | The export runs | The payload is machine-readable and preserves field semantics required for portability | |
| AC-003 | The underlying ledger contains records for multiple subjects | The export scope is computed | Data belonging to other subjects is excluded or pseudonymized before delivery | |

## Dependencies
- [[AUDT-001]]

## Notes
- Rationale: GDPR Articles 15(1), 15(3), and 20 require access and portability in a commonly used electronic form.
- Exports should preserve integrity evidence for audit review while remaining limited to the requesting subject's data.
