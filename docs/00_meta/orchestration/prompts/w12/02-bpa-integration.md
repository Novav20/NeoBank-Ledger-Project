---
status: pending
issued_by: Gemini CLI
issued_date: 2026-03-20
response_ref: docs/00_meta/orchestration/responses/w12/02-bpa-integration.md
---
# Copilot Instruction: BPA Integration (Batch 01)
**Location:** docs/00_meta/orchestration/prompts/w12/02-bpa-integration.md

---

## Task Context
We have completed the **Batch 01 Meta-Analysis** (`.../responses/w12/01-batch-01-meta-analysis.md`), which synthesized our foundational research on B2B Ledgers. The next critical step is to integrate these evidence-based findings into our primary architectural artifact, the **BPA Report**.

## Objectives
1.  Enrich the `docs/02_analysis/bpa/BPA_Report.md` with the detailed findings from the meta-analysis.
2.  Ensure that the updated BPA Report maintains traceability by including the inline citations `([[author_year|Author et al., YEAR]])`.
3.  Add a versioning header to the BPA Report.

## Constraints & Requirements
-   **Input Source**: `docs/00_meta/orchestration/responses/w12/01-batch-01-meta-analysis.md`.
-   **Target File**: `docs/02_analysis/bpa/BPA_Report.md`.
-   **Output Path**: Overwrite the existing `docs/02_analysis/bpa/BPA_Report.md` with the integrated version.

## Specific Instructions
1.  **Add YAML Header**: At the very top of `BPA_Report.md`, add a YAML frontmatter block. Set `version: 1.0` and `last_modified: 2026-03-20`.
2.  Read the content of both the meta-analysis report and the current BPA report.
3.  Go section by section through the BPA report:
    -   **Introduction & Context**: Integrate the synthesized "Stakeholder Landscape".
    -   **As-Is Analysis**: Use the "Foundational Logic (As-Is)" section to add detail.
    -   **Pain Points**: Expand this section using the "System Entropy" findings.
4.  Use the "Non-negotiable Principles" to create a new, detailed **Section 4: System Requirements & Business Rules**.
5.  Preserve all inline citations from the meta-analysis.

---
