---
status: pending
issued_by: Gemini CLI
issued_date: 2026-04-14
response_ref: docs/00_meta/orchestration/responses/w16/03-gap-resolution-and-audit-req.md
---
# Task: Gap Resolution and Audit Requirement Generation

## Task Context
Based on research findings from the BPA (Boukhatmi & Van Opstal, 2025; Mashiko et al., 2025; Sonnino, 2021), we have resolved the "Audit Proof Delivery" and "Regulatory Access" gaps. We now need to update our Gap Register and formalize the corresponding Functional Requirement for the Audit system.

## Objectives
1.  **Resolve Gaps**: Update `docs/02_analysis/requirements/use_cases/gaps.md` to move "Audit proof delivery mode" and "Regulatory access scope" to a "Resolved Gaps" section.
2.  **Generate FR-005**: Create `docs/02_analysis/requirements/functional_requirements/FR-005.md` for **Periodic Audit Checkpointing**.

## Specific Instructions

### 1. Gap Resolution Details
Update the Gaps file with the following resolutions:
- **Audit proof delivery mode**: Hybrid model. Shards periodically publish signed hash-chain checkpoints (Merkle roots) to an evidence repository (Push). Individual transaction proofs are served via API (Pull).
- **Regulatory access scope**: Passive Oversight. Regulators utilize "Observer Nodes" to post-validate the published checkpoints without participating in the consensus/settlement process.
- **Reference**: BPA (Boukhatmi & Van Opstal, 2025; Mashiko et al., 2025; Sonnino, 2021).

### 2. Functional Requirement (FR-005)
- **ID**: FR-005
- **Title**: Periodic Audit Checkpointing
- **Statement**: The Ledger system MUST generate and publish a signed cryptographic checkpoint (Merkle root) of the Event Log at defined intervals (epochs) to facilitate continuous oversight.
- **Rationale**: Grounded in BPA 4.1 and the need for evidence-grade logging and non-repudiation (Boukhatmi & Van Opstal, 2025). Supports UC-03.
- **Risk Level**: High.
- **Priority**: High.

## Constraints & Requirements
- Use the updated requirement template (`docs/00_meta/orchestration/templates/requirement-specification.md`).
- Ensure 100% traceability in the frontmatter.
- Maintain the atomic nature of the requirement.
