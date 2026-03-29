---
status: pending
issued_by: Gemini CLI
issued_date: 2026-03-29
response_ref: docs/00_meta/orchestration/responses/w13/03-batch-03-meta-analysis.md
---
# Copilot Instruction: Iterative Meta-Analysis (Batch 03 - NFRs & Compliance)
**Location:** docs/00_meta/orchestration/prompts/w13/03-batch-03-nfr-compliance.md

---

## Task Context
We are finalizing the **Batch 03 Research** focused on Non-Functional Requirements (NFRs) and Compliance for the B2B Ledger API. We have 37 split snippets. To ensure high-fidelity analysis and avoid skipping technical details, we are using an **Iterative "Sliding Window" Protocol**.

## Objectives
1.  **Iterative Processing**: Analyze exactly **5 unprocessed sources** from `docs/02_analysis/research/w13/batch-03/` per turn.
2.  **Priority Assignment**: Update the `priority` field (high/medium/low) in the source's YAML frontmatter based on architectural relevance to a B2B Ledger.
3.  **Meta-Analysis Synthesis**: Draft (or append to) the meta-analysis report using the `meta-analysis-template.md` structure.
4.  **Metadata Update**: Set `processed: true` and `cited_in_meta_analysis: true` in the processed sources.

## Constraints & Requirements
-   **Limit**: Process ONLY 5 sources per turn.
-   **Traceability**: Maintain inline citations `([[author_year|Author, YEAR]])`.
-   **Output Path**: `docs/00_meta/orchestration/responses/w13/03-batch-03-meta-analysis.md`.
-   **No Emojis**.

## Specific Instructions
1.  **Selection**: Identify 5 files in the batch-03 folder where `processed: false`.
2.  **Analysis**:
    -   Evaluate technical depth regarding: Regulatory compliance, Latency/Throughput NFRs, and Security standards.
    -   Assign `priority` in the source file.
3.  **Synthesis**:
    -   If this is the first turn, initialize the report using the template.
    -   If the report exists, add an `### UPDATE-XX` block (following the Delta Update protocol) to integrate these 5 sources into the existing synthesis.
4.  **Metadata Writeback**: Update the source files' YAML block to reflect they are now `processed`.
5.  **Completion**: Stop after 5 sources and summarize which authors were processed. **Do not continue until the user validates this block.**

---
