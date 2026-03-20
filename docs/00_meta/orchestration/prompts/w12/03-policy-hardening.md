---
status: pending
issued_by: Gemini CLI
issued_date: 2026-03-20
response_ref: docs/00_meta/orchestration/responses/w12/03-policy-hardening.md
---
# Copilot Instruction: Harden Session Logging Protocol
**Location:** docs/00_meta/orchestration/prompts/w12/03-policy-hardening.md

---

## Task Context
We have identified critical flaws in our session management protocol, risking data loss. We must harden the `OrchestrationPolicy.md` with two new explicit rules to ensure a robust, "Shared Memory" workflow.

## Objectives
1.  Update the `Session Logging` section in `docs/00_meta/OrchestrationPolicy.md`.
2.  Add the **"Archive-First Principle"** for session state updates.
3.  Add the **"Append-Only Discipline"** for daily logs, using a sequential update counter.

## Constraints & Requirements
-   **Target File**: `docs/00_meta/OrchestrationPolicy.md`.
-   **Clarity**: The rules must be unambiguous.
-   **Output**: Overwrite the existing `OrchestrationPolicy.md` file.

## Specific Instructions
1.  Read `docs/00_meta/OrchestrationPolicy.md`.
2.  Find the `## Session Logging` section.
3.  Add two new sub-sections or bullet points with the following rules:
    *   **Rule 1 Title**: The Archive-First Principle.
    *   **Rule 1 Text**: "At the end of a session, the daily log (`.../logs/weekly/wXX/YYYY-MM-DD.md`) MUST be created or updated with the session's completed work *before* the live `session-state.md` is updated for the next session's handover. This ensures no work is lost if a session is unexpectedly terminated."
    *   **Rule 2 Title**: Append-Only Discipline for Daily Logs.
    *   **Rule 2 Text**: "If multiple distinct milestones are achieved within the same day's session, the `YYYY-MM-DD.md` file for that day MUST be appended to, not overwritten. New entries should be separated by a horizontal rule (`---`) and a sequential `### UPDATE-02`, `### UPDATE-03`, etc., header. This creates a running, deterministic log of the day's full activities."
4.  Increment the `version` header of the policy file to `1.1`.

---
