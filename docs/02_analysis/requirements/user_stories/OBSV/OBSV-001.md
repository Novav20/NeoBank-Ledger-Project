---
type: user-story
version: 1.0
last_edited: 2026-04-20
status: draft
story_id: OBSV-001
epic_id: OBSV
epic_name: Operational Observability
title: Operate the ledger with BFT health, storage, and recovery guardrails
persona: System Operator
goal: see the system health, availability, and storage controls needed to keep the ledger within its safe operating envelope
business_value: detect liveness issues early, keep storage bounded, and maintain the configuration discipline required for stable operations
acceptance_format: gherkin-table
source:
  - docs/02_analysis/requirements/epics.md#OBSV
  - docs/02_analysis/requirements/non_functional_requirements/NFR-001.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-004.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-005.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-011.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-012.md
  - docs/02_analysis/requirements/non_functional_requirements/NFR-015.md
  - docs/02_analysis/research/w17/Technical-and-Functional-Requirements-Specification_GDPR-Compliance-for High-Integrity-Financial-Ledgers.md
  - docs/02_analysis/research/w17/High-Integrity Financial Ledger Requirements from GDPR.csv
derived_from:
  - docs/02_analysis/requirements/epics.md#OBSV
related_use_cases:
  - UC-05
related_fr: []
related_nfr:
  - "[[NFR-001]]"
  - "[[NFR-004]]"
  - "[[NFR-005]]"
  - "[[NFR-011]]"
  - "[[NFR-012]]"
  - "[[NFR-015]]"
depends_on: []
implementation_path: src/NeoBank.Ledger.Infrastructure/
test_path: tests/NeoBank.Ledger.Infrastructure.Tests/
sprint: Sprint 01
priority: Could
estimate: 13
verification:
  - acceptance test
  - fault-injection test
  - monitoring drill
risk_level: Critical
notes:
  - Parent backlog slice for health, timing, storage, and migration governance.
---

# OBSV-001: Operate the Ledger with BFT Health, Storage, and Recovery Guardrails

## Story Statement
As a System Operator, I want the ledger to expose availability, timing, storage, and governance guardrails, so that I can keep the platform inside its safe operating envelope.

## Context
This parent story covers the operational observability epic. It groups the liveness, timer, latency, migration, dependency, and storage controls that operators need to keep the system stable.

## Acceptance Criteria
Use a Gherkin-inspired table. Keep one row per atomic criterion.

| AC ID | Given | When | Then | Notes |
| --- | --- | --- | --- | --- |
| AC-001 | The BFT cluster is under observation | A leader fails | The system continues to make progress with 3F+1 nodes and completes view change within 30 seconds | |
| AC-002 | The consensus timer is configured | The timer baseline is reviewed | The initial timer is at least 3σ above expected consensus time | |
| AC-003 | The tiered SLA is measured | Intra-continental and WAN traffic are benchmarked | The engine stays below 100ms intra-continental and within the 1.5s WAN ceiling | |
| AC-004 | The cryptography roadmap is reviewed | The security posture is assessed | The system maintains a documented post-quantum migration roadmap | |
| AC-005 | Open-source dependencies are triaged | Vulnerability response is evaluated | The project follows a defined OSS dependency SLA and CVE patching discipline | |
| AC-006 | The storage policy is reviewed | Epoch checkpointing is inspected | The system enforces epoch checkpoints and a design-time per-node storage cap | |
| AC-007 | A non-privileged operator opens the default monitoring view | Operational telemetry is requested | Only least-privilege fields are returned and restricted data remains hidden unless elevated access is granted | |
| AC-008 | Ledger data is stored or transmitted between components | The system persists or relays the data | Encryption at rest and in transit is enforced according to the security policy | |

## Notes
- This parent story covers the operational controls that keep availability, storage, and governance visible to the operator.
- Rationale: GDPR Articles 25 and 32 require restrictive defaults, confidentiality, and encrypted processing for operational data that may contain sensitive ledger metadata.