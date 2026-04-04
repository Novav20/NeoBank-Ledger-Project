---
status: pending
issued_by: Gemini CLI
issued_date: 2026-03-24
response_ref: docs/00_meta/orchestration/responses/w13/02-bpa-integration.md
---
# Copilot Instruction: BPA Integration (Batch 02 Technicals)
**Location:** docs/00_meta/orchestration/prompts/w13/02-bpa-integration-batch02.md

---

## Task Context
We have completed the **Batch 02 Meta-Analysis**, which provides the technical implementation patterns for our B2B Ledger. The next step is to integrate these cited findings into our `BPA_Report.md` to bridge domain requirements with architectural reality.

## Objectives
1.  Enrich the `docs/02_analysis/bpa/BPA_Report.md` with the technical findings from Batch 02.
2.  Update `docs/02_analysis/bpa/Glossary.md` with any new terms or definitions needed for Batch 02 concepts.
3.  Create a new **Section 5: Technical Architecture & "To-Be" Model**.
4.  Ensure all new information is cited using the existing `[[...]]` format.

## Constraints & Requirements
-   **Input Source**: `docs/00_meta/orchestration/responses/w13/01-batch-02-meta-analysis.md`.
-   **Target File**: `docs/02_analysis/bpa/BPA_Report.md`.
-   **Output Path**: Overwrite the existing `docs/02_analysis/bpa/BPA_Report.md`.

## Specific Instructions
1.  Read the content of both the meta-analysis report and the current BPA report.
2.  **Update BPA Version**: Increment the `version` in the YAML frontmatter of `BPA_Report.md` to `1.1` and update `last_modified` to today's date.
3.  **Integrate "Non-negotiable Principles"**:
    *   Find **Section 4: System Requirements & Business Rules** in the BPA.
    *   Enhance this section by integrating the more detailed principles from the Batch 02 meta-analysis, such as the need for an "explicit finality model," "conflict-aware ordering," and "cross-shard atomicity." Preserve citations.
4.  **Create Section 5**:
    *   Add a new `## 5. Technical Architecture & "To-Be" Model` section to the BPA Report.
    *   Use the "Cross-Source Synthesis" and "Impact on Artifacts" sections from the meta-analysis to populate this new section.
    *   Describe the "To-Be" model in terms of **Event Sourcing**, a **sharded architecture**, and a **Fabric-like Execute-Order-Validate** pipeline.
    *   Include a new Mermaid diagram for the "To-Be" flow, showing the interaction between the sequencer, validation shards, and the append-only log.

---
