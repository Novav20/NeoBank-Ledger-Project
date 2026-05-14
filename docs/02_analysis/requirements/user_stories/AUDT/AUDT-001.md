---
type: user-story
version: 1.0
last_edited: 2026-04-20
status: draft
story_id: AUDT-001
epic_id: AUDT
epic_name: Integrity & Audit Vault
title: Preserve auditable ledger integrity and proof-ready finality
persona: Auditor
goal: verify that ledger history remains balanced, finality is distinguishable, and each block carries durable proof material
business_value: keep audit evidence complete, regulator-friendly, and independently verifiable
acceptance_format: gherkin-table
source:
  - docs/02_analysis/requirements/epics.md#AUDT
  - docs/02_analysis/requirements/functional_requirements/FR-003.md
  - docs/02_analysis/requirements/functional_requirements/FR-005.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-006.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-008.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-017.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-018.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-019.md
  - docs/02_analysis/research/w17/Technical-and-Functional-Requirements-Specification_GDPR-Compliance-for High-Integrity-Financial-Ledgers.md
  - docs/02_analysis/research/w17/High-Integrity Financial Ledger Requirements from GDPR.csv
derived_from:
  - docs/02_analysis/requirements/epics.md#AUDT
related_use_cases:
  - UC-03
related_fr:
  - "[[FR-003]]"
  - "[[FR-005]]"
related_nfr:
  - "[[NFR-006]]"
  - "[[NFR-008]]"
  - "[[NFR-017]]"
  - "[[NFR-018]]"
  - "[[NFR-019]]"
depends_on:
  - "[[TXEN-003]]"
  - "[[PROJ-001]]"
implementation_path: src/NeoBank.Ledger.Infrastructure/Persistence/Repositories/RocksDbLedgerUnitOfWork.cs
test_path: tests/NeoBank.Ledger.Infrastructure.Tests/Persistence/RocksDbLedgerUnitOfWorkTests.cs
sprint: Sprint 01
priority: Must
estimate: 13
verification:
  - acceptance test
  - integration test
  - audit-proof review
  - formal-model check
risk_level: Critical
notes:
  - Parent backlog slice for integrity proofs and audit evidence.
---

# AUDT-001: Preserve Auditable Ledger Integrity and Proof-Ready Finality

## Story Statement
As an Auditor, I want the ledger to preserve balanced transactions and distinguish committed from audited finality, so that I can verify evidence without disturbing the committed history.

## Context
This parent story covers the audit vault epic. It combines the integrity checks, checkpointing, and proof obligations that make ledger history independently defensible.

## Acceptance Criteria
Use a Gherkin-inspired table. Keep one row per atomic criterion.

| AC ID | Given | When | Then | Notes |
| --- | --- | --- | --- | --- |
| AC-001 | A transaction does not satisfy double-entry balance | The transaction is submitted | The system rejects it before commit | |
| AC-002 | A trade event is recorded for regulatory review | Metadata is written | The record includes an LEI, a UTI, and a 1ms timestamp | |
| AC-003 | A transaction has been committed | Audit reconciliation runs | The system distinguishes committed finality from audited finality | |
| AC-004 | A formal verification run is executed | Proof artifacts are generated | The artifacts demonstrate fork-freedom and safety-error-freedom | |
| AC-005 | A block is published for oversight | Quorum evidence is attached | The block is independently auditable through a QC-backed record | |
| AC-006 | A processing record or lineage request is made | The audit evidence is queried | The system returns machine-readable lineage for the request, transformation, and disclosure history | |

## Notes
- This parent story covers the audit-grade evidence chain and the proof obligations that support it.
- Rationale: GDPR Article 30 requires records of processing activities, so the audit vault must expose machine-readable lineage rather than only preserving cryptographic proof material.