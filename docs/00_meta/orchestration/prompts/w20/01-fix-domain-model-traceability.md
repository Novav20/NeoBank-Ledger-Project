---
status: pending
issued_by: Gemini CLI
issued_date: 2026-05-11
response_ref: docs/00_meta/orchestration/responses/w20/01-fix-domain-model-traceability.md
---
# Task: Synchronize Domain Model Physical Traceability

## Task Context
During a re-engagement audit of the `docs/03_architecture/Domain-Entity-Model.md`, we identified that several properties defined in the **ERD** and **Class Diagram** are missing from the **Physical Column Traceability** table in Section 5. We need to ensure 100% documentation alignment to maintain ISO/IEC 29110 standards.

## Objectives
1.  **Analyze Inconsistencies**: Compare the `TRANSACTIONS`, `EVENTS`, `BALANCES`, and `PARTIES` definitions in the Mermaid ERD section with the **Physical Column Traceability** table.
2.  **Restore Missing Rows**: Specifically, add the following missing columns to the traceability table:
    - `TRANSACTIONS.end_to_end_id`
    - `TRANSACTIONS.message_definition_id`
    - `TRANSACTIONS.message_function`
3.  **Cross-Reference Drafts**: Read the archived drafts in `docs/00_meta/archive/architecture_drafts/` to ensure the "Source Note" and "Critical Synthesis" columns for these missing properties are accurate and grounded in the original research.
4.  **Full Audit**: Check all other entities in the ERD to ensure NO physical column is left undocumented in the traceability table.

## Constraints & Requirements
- **Format**: Maintain the existing Markdown table style.
- **Traceability**: Every row added must have a valid "Source Note" (e.g., ISO 20022, MiFID II, or specific ADR).
- **Physical Types**: Ensure the types (e.g., `nvarchar(35)`) match the ERD section exactly.

## Implementation Path
- `docs/03_architecture/Domain-Entity-Model.md`
