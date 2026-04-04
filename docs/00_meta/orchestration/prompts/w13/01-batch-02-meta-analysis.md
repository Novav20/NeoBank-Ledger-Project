---
status: pending
issued_by: Gemini CLI
issued_date: 2026-03-23
response_ref: docs/00_meta/orchestration/responses/w13/01-batch-02-meta-analysis.md
---
# Copilot Instruction: Batch 02 Incremental Meta-Analysis (Technicals)
**Location:** docs/00_meta/orchestration/prompts/w13/01-batch-02-meta-analysis.md

---

## Task Context
We are performing a **two-phase meta-analysis** on the Batch 02 technical sources to prevent LLM context overload. This prompt orchestrates that incremental synthesis.

## Objectives
1.  **Phase 1**: Create a baseline meta-analysis from a small, high-priority set of sources.
2.  **Phase 2**: Update the baseline with findings from the remaining sources.
3.  Add a `priority` flag to all source files for focused synthesis.
4.  Add `processed: true/false` to each source’s YAML frontmatter when that source is extracted into the meta-analysis, to support traceability and batch progress tracking.

## Constraints & Requirements
-   **Input Folder**: `docs/02_analysis/research/w13/batch-02/`
-   **Structure**: Strictly follow the structure defined in `docs/02_analysis/research/w12/artifacts/meta-analysis-template.md`.
-   **Output Path**: `docs/00_meta/orchestration/responses/w13/01-batch-02-meta-analysis.md`.

## Specific Instructions

### **Part 1: Prioritization (Manual User Task)**
*(This part is for the user to complete before running Part 2)*
1.  Go through all `.md` files in the `batch-02/` folder.
2.  Identify the **Top 5** most architecturally significant papers.
3.  In the YAML frontmatter of those 5 files, add the property: `priority: high`.
4.  For all other files in the folder, add the property: `priority: medium`.

### **Part 2: Incremental Synthesis (Copilot Task)**
1.  **Phase 1 (High Priority Synthesis)**:
    *   Read **only** the files in the input folder where `priority: high`.
    *   Generate a complete meta-analysis report using the standard template.
2.  **Phase 2 (Medium Priority Update)**:
    *   Now, read **only** the files where `priority: medium`.
    *   **Update** the meta-analysis report you just generated. Do not overwrite it.
    *   For each section (e.g., "Event Sourcing", "Consistency"), append any new, non-redundant findings from the medium-priority sources.
    *   Update the "Gaps & Next Batch Direction" section based on the complete set of sources.
3.  Ensure the final report in the output path is a single, cohesive document.

---
