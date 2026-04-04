---
status: pending
issued_by: Gemini CLI
issued_date: 2026-03-29
response_ref: docs/00_meta/orchestration/responses/w13/04-bpa-finalization.md
---
# Copilot Instruction: Final BPA Integration (Batch 03 - NFRs & Compliance)
**Location:** docs/00_meta/orchestration/prompts/w13/04-bpa-integration-batch03.md

---

## Task Context
We have completed the full 37-source Meta-Analysis for **Batch 03** (`.../responses/w13/03-batch-03-meta-analysis.md`). This batch provides the definitive Non-Functional Requirements (NFRs) and Regulatory Compliance targets for the B2B Ledger API. We must now integrate this into the `BPA_Report.md` to finalize version 1.2.

## Objectives
1.  **Selective Integration**: Deepen the `BPA_Report.md` using ONLY the "High Signal" and "Medium Signal" findings from the Meta-Analysis. **Ignore all sources classified as "Low Signal".**
2.  **NFR Registry Formalization**: Create a new, structured section in the BPA for the **Consolidated NFR Registry**.
3.  **Traceability**: Maintain all inline citations `([[author_year|Author, YEAR]])`.
4.  **Version Lock**: Update the report version to **1.2**.

## Constraints & Requirements
-   **Core Anchor**: Use `sonnino_2021` for performance and `chuen_2017` for compliance.
-   **No Hallucinations**: Do not add requirements not found in the Meta-Analysis.
-   **Tone**: Senior Architect level. Professional and rigorous.
-   **Glossary**: Reference terms from `docs/02_analysis/bpa/Glossary.md` where it adds clarity for a non-domain reader.

## Specific Instructions
1.  **Update YAML Header**: Change `version` to `1.2` and `last_modified` to `2026-03-29`.
2.  **Section 1 (Stakeholders)**: Integrate the "Regulators & Auditors" role transition from post-hoc to real-time monitors.
3.  **Section 3 (Pain Points)**: Add the "Nth-Party Risk" and "MEV/Orderer Collusion" risks identified in Block 03.
4.  **Section 4 (System Requirements & Business Rules)**:
    -   Rename or expand this to include the **Consolidated NFR Registry**.
    -   Explicitly include the **ISO 20022** and **MiFID II (LEI/UTI)** compliance mandates.
    -   Include the **"Committed vs. Audited Finality"** distinction.
5.  **Section 6 (Metrics & Value Expected)**:
    -   Populate this section using the Benchmark Table from the Meta-Analysis.
    -   Define the **Tiered SLA**: <100ms for intra-cluster, <1.5s for WAN.
6.  **Section 7 (Gap Analysis - Future Work)**:
    -   Mention the identified **GDPR vs. Immutability** conflict as a pending design decision.

---
