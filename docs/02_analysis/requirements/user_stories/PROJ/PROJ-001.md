---
type: user-story
version: 1.1
last_edited: 2026-05-14
status: draft
story_id: PROJ-001
epic_id: PROJ
epic_name: Balance & State Projection
title: Project balances into an integer-precise world state
persona: Fintech Partner
goal: query a real-time world state that preserves integer precision and is provisioned with the right storage and shard profile
business_value: keep balance views authoritative, prevent rounding drift, and make the projection layer deployable at scale
acceptance_format: gherkin-table
source:
  - docs/02_analysis/requirements/epics.md#PROJ
  - docs/02_analysis/requirements/functional_requirements/FR-004.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-013.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-014.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-020.md
derived_from:
  - docs/02_analysis/requirements/epics.md#PROJ
related_use_cases:
  - UC-02
related_fr:
  - "[[FR-004]]"
related_nfr:
  - "[[NFR-013]]"
  - "[[NFR-014]]"
  - "[[NFR-020]]"
depends_on:
  - "[[TXEN-002]]"
implementation_path: src/NeoBank.Ledger.Infrastructure/
test_path: tests/NeoBank.Ledger.Infrastructure.Tests/Persistence/RocksDbLedgerUnitOfWorkTests.cs
sprint: Sprint 01
priority: Should
estimate: 5
verification:
  - acceptance test
  - configuration review
risk_level: High
notes:
  - Parent backlog slice for balance projection and deployment sizing.
---

# PROJ-001: Project Balances into an Integer-Precise World State

## Story Statement
As a Fintech Partner, I want projected balances to remain integer-precise and deployed on the right storage and shard profile, so that the world state is trustworthy and scalable.

## Context
This parent story covers the state projection epic. It ensures the ledger presents balances in a real-time world state without rounding drift and with a deployment profile that supports growth.

## Acceptance Criteria
Use a Gherkin-inspired table. Keep one row per atomic criterion.

| AC ID | Given | When | Then | Notes |
| --- | --- | --- | --- | --- |
| AC-001 | A monetary amount enters the projection layer | The value is stored or computed | The value remains in integer smallest-unit form | |
| AC-002 | The default ledger storage is being provisioned | The database is configured | RocksDB is selected and CouchDB or private data collections require documented justification | |
| AC-003 | A deployment plan is being reviewed | The m-node ratio is checked | The shard ratio stays between 20% and 25% | |
| AC-004 | A 1,000-node deployment target is sized | The shard count is configured | The plan uses 7 PBFT shards | |

## Notes
This parent story covers the projection-layer data contract and its deployment guardrails.