---
status: pending
issued_by: Gemini CLI
issued_date: 2026-03-20
response_ref: docs/00_meta/orchestration/responses/w12/04-policy-refinement.md
---
# Copilot Instruction: Refine Session Logging Protocol (Delta Updates)
**Location:** docs/00_meta/orchestration/prompts/w12/04-delta-update-policy.md

---

## Task Context
Our "Append-Only Discipline" for daily logs is not yet deterministic. We need to define a specific, smaller format for *updates* to prevent messy, repetitive logs.

## Objectives
1.  Update the `Session Logging` section in `docs/00_meta/OrchestrationPolicy.md`.
2.  Define a **"Delta Update Block"** format for appending to existing daily logs.

## Constraints & Requirements
-   **Target File**: `docs/00_meta/OrchestrationPolicy.md`.
-   **Clarity**: The format must be explicit and easy for an agent to parse and generate.
-   **Output**: Overwrite the existing `OrchestrationPolicy.md` file.

## Specific Instructions
1.  Read `docs/00_meta/OrchestrationPolicy.md` (v1.1).
2.  Find the "Append-Only Discipline for Daily Logs" rule.
3.  Modify the rule text to specify the "Delta Update" format. It should state that appends MUST use a format like this:

    ```markdown
    ---
    ### UPDATE-02
    
    **New Key Decision(s):**
    - [Decision text]
    
    **New Completed Task(s):**
    - [x] [Task text]
    
    **Revised AI Handover Context:**
    [A brief, updated handover statement]
    ```
4.  The rule should clarify that the full log template is only for the *first* entry of the day.
5.  Increment the `version` header of the policy file to `1.2`.

---
