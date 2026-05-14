---
type: user-story
version: 1.0
last_edited: 2026-04-15
status: draft
story_id: TXEN-001
epic_id: TXEN
epic_name: Transaction Engine (EOV)
title: Authenticate and authorize a transaction command
persona: Fintech Partner
goal: submit a transaction command through an authenticated and authorized channel
business_value: ensure the ledger only accepts trusted command traffic and can start the EOV flow safely
acceptance_format: gherkin-table
source:
  - docs/02_analysis/requirements/epics.md#TXEN
  - docs/02_analysis/requirements/functional_requirements/FR-001.md
  - docs/02_analysis/bpa/BPA_Report.md#L24
  - docs/02_analysis/bpa/BPA_Report.md#L131
derived_from:
  - docs/02_analysis/requirements/epics.md#TXEN
related_use_cases:
  - UC-01
related_fr:
  - "[[FR-001]]"
related_nfr: []
depends_on: []
implementation_path: src/NeoBank.Ledger.Api/
test_path: tests/NeoBank.Ledger.Infrastructure.Tests/
sprint: Sprint 01
priority: Must
estimate: 3
verification:
  - acceptance test
risk_level: Critical
notes: []
---

# TXEN-001: Authenticate and Authorize a Transaction Command

## Story Statement
As a Fintech Partner, I want my transaction command to be authenticated and authorized before processing, so that only trusted requests enter the ledger.

## Context
This story covers the first step of the Transaction Engine epic. It gives the external partner a secure entry point into the EOV flow and maps directly to the access-control requirement already derived from the BPA.

## Acceptance Criteria
Use a Gherkin-inspired table. Keep one row per atomic criterion.

| AC ID | Given | When | Then | Notes |
| --- | --- | --- | --- | --- |
| AC-001 | The caller is authenticated and authorized | The partner submits a transaction command | The Ledger API accepts the request for downstream processing | |
| AC-002 | The caller is unauthenticated | The request reaches the gateway | The system rejects the command before any transaction processing begins | |
| AC-003 | The caller is authenticated but unauthorized | The request reaches the gateway | The system rejects the command before any transaction processing begins | |

## Notes
Normalization and event shaping are intentionally out of scope here and will appear in TXEN-002.
