---
status: pending
issued_by: Gemini CLI
issued_date: 2026-04-20
response_ref: docs/00_meta/orchestration/responses/w17/03-gdpr-backlog-expansion.md
---
# Task: GDPR Backlog Expansion

## Task Context
The GDPR Gap Analysis (02-gdpr-gap-analysis.md) has identified specific operational and technical gaps in our backlog. We need to update existing stories and create new ones to ensure "Privacy by Design" and legal robustness (Erasure Exclusions).

## Objectives
1.  **Update Existing Stories**:
    - **TXEN-002**: Add AC for data minimization (store only mandatory PII) and pseudonymized identifiers.
    - **OBSV-001**: Add AC for privacy-by-default (restrictive access) and encryption at rest/transit (Art 32).
    - **AUDT-001**: Add AC for processing-record lineage (Art 30).
2.  **Create New Stories** (in `docs/02_analysis/requirements/user_stories/AUDT/`):
    - **AUDT-002: Subject Access and Data Portability**: (Art 15/20) Machine-readable export of a subject's data. Priority: `Should`.
    - **AUDT-003: Erasure Exclusions and Legal-Hold**: (Art 17.3) Workflow to block redaction if data is needed for financial audit or legal claims. Priority: `Should`.

## Specific Instructions
- Use the standard `user-story-template.md`.
- Ensure all new stories reference the relevant GDPR Articles in the `source` and `rationale`.
- Maintain the `Must` priority for the core path; mark new GDPR rights as `Should` to protect the MVP scope.
- Reference [[ADR-001-GDPR-Compliance]] as the enabling mechanism for the erasure stories.

## Expected Output
- Updated `.md` files for TXEN-002, OBSV-001, and AUDT-001.
- New files for AUDT-002 and AUDT-003.
