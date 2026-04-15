---
type: user-story
version: 1.0
last_edited: 2026-04-15
status: draft
story_id: TXEN-002
epic_id: TXEN
epic_name: Transaction Engine (EOV)
title: Normalize a validated transaction command
persona: Fintech Partner
goal: turn a validated transaction command into a structured event that the ledger can process deterministically
business_value: reduce manual handling and keep the transaction intake path machine-readable and consistent
acceptance_format: gherkin-table
source:
  - docs/02_analysis/requirements/epics.md#TXEN
  - docs/02_analysis/requirements/functional_requirements/FR-002.md
  - docs/02_analysis/bpa/BPA_Report.md#L24
  - docs/02_analysis/bpa/BPA_Report.md#L131
derived_from:
  - docs/02_analysis/requirements/epics.md#TXEN
related_use_cases:
  - UC-01
related_fr:
  - "[[FR-002]]"
related_nfr:
  - "[[NFR-COMP-01]]"
depends_on:
  - "[[TXEN-001]]"
priority: Must
estimate: 5
verification:
  - acceptance test
risk_level: High
notes: []
---

# TXEN-002: Normalize a Validated Transaction Command

## Story Statement
As a Fintech Partner, I want a validated transaction command to be normalized into a structured event, so that the ledger can process it deterministically.

## Context
This story covers the second step of the Transaction Engine epic. It turns trusted inbound intent into a structured event and sets up the remainder of the EOV pipeline.

## Acceptance Criteria
Use a Gherkin-inspired table. Keep one row per atomic criterion.

| AC ID | Given | When | Then | Notes |
| --- | --- | --- | --- | --- |
| AC-001 | A transaction command has passed authentication and authorization | The normalization step runs | The system produces a structured internal event | |
| AC-002 | A structured internal event has been produced | Downstream execution or ordering begins | Normalization is complete before further processing starts | |
| AC-003 | The structured event is reviewed against the project message model | The schema mapping is checked | The payload remains compatible with the ISO 20022 alignment path | Specific ISO 20022 profiles remain pending. |

## Dependencies
- TXEN-001.

## Notes
Specific ISO 20022 message profiles remain placeholders until the schema research is completed.
