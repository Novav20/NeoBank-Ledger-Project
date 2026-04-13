---
status: pending
issued_by: Gemini CLI
issued_date: 2026-04-13
response_ref: docs/00_meta/orchestration/responses/w16/01-use-case-specification.md
---
# Task: Detailed Use Case Specification Extraction

## Task Context
We are transitioning from high-level research (BPA) to technical system design. To ensure professional quality (ISO/IEC 29110), we need to formalize the **Actor Goals** and **System Behaviors** through detailed Use Case Specifications before deriving Functional Requirements.

## Objectives
1.  **Folder Restructure**: Move `docs/02_analysis/requirements/use-cases.md` to `docs/02_analysis/requirements/use_cases/specifications.md`.
2.  **Gap Analysis**: Create `docs/02_analysis/requirements/use_cases/gaps.md` to track missing information or ambiguities identified during extraction.
3.  **Detailed Specification**: Generate full specifications for the following Use Cases:
    *   UC-01: Initiate Transaction (EOV Flow)
    *   UC-02: Query Materialized Balance
    *   UC-03: Validate Integrity Proof (Hash-Chain Audit)
    *   UC-04: Reconcile External Liquidity
    *   UC-05: Monitor System Health

## Constraints & Requirements
- **Language**: English.
- **Grounding**: 100% traceability to `docs/02_analysis/bpa/BPA_Report.md` and `docs/02_analysis/requirements/bpmn/processes.md`.
- **Systematic Research**: If the BPA text is insufficient to define a specific step, alternate flow, or precondition, you MUST traverse the research artifacts in `docs/02_analysis/research/` to find the corresponding cited evidence.
- **Format**: Use the professional Markdown table format defined below.
- **Research Integrity**: If a behavior remains unsupported after reviewing the BPA, BPMN notes, and cited research, do NOT invent it. Record the ambiguity in `gaps.md`.


## Use Case Table Template
| **Element** | **Detail** |
| :--- | :--- |
| **ID** | [e.g., UC-01] |
| **Name** | [Goal-oriented name] |
| **Primary Actor** | [From BPA Section 1.3] |
| **Secondary Actors** | [System services or other stakeholders] |
| **Description** | [Brief summary of the goal] |
| **Preconditions** | [Bulleted list of requirements before start] |
| **Main Flow** | [Numbered steps mapping the BPMN To-Be lifecycle] |
| **Alternate Flows** | [AF-001: Exception Name. Document numbered steps for the deviation] |
| **Postconditions** | [Bulleted list of system state after completion] |
| **Associated Requirements** | [Placeholders for FR IDs, e.g., FR-LDR-01] |

## Specific Instructions
1.  **Read BPA Section 5.2**: Ensure the Main Flow for UC-01 follows the **Execute -> Order -> Validate -> Commit** pipeline exactly.
2.  **Read BPA Section 4.1**: Extract Preconditions (Double-Entry Balance, Integer Precision) and Postconditions (Immutable Log, World State update).
3.  **Identify Gaps**: If the BPA doesn't specify what happens if a "Validation Shard" fails, add this as a gap in `gaps.md` rather than assuming a behavior.
4.  **Review BPMN**: Use `docs/02_analysis/requirements/bpmn/actors-stakeholders.md` to ensure actor naming is consistent.
