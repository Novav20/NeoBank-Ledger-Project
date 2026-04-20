---
title: Orchestration Policy
version: 1.2
---

# Orchestration Policy (single source of truth)

Purpose: centralize evolving orchestration rules, session bootstrap, escalation and rejection protocols, and naming conventions so `GEMINI.md` and `.github/copilot-instructions.md` remain stable and reference a single authoritative policy file.

## Where to use this file
- Reference from `GEMINI.md` and `.github/copilot-instructions.md` for any operational or protocol-level content.

## Session Bootstrap
- At session start, read `docs/00_meta/orchestration/logs/session-state.md` and the `Last Daily Log` it references.
- `session-state.md` is the live snapshot and is overwritten at session start. Weekly archives live under `docs/00_meta/orchestration/logs/weekly/wNN/`.

## Naming & Layout Conventions
- Week folders: lowercase `wNN` (e.g., `w11`).
- Orchestration folders: `docs/00_meta/orchestration/{templates,prompts,responses,logs}`.
- Local-only archives: `docs/00_meta/archive/` (may be gitignored).

## Escalation & Rejection Protocol
- If an execution engine (Copilot) flags a prompt flaw, it should still execute the prompt but document concerns in an `Analysis / Findings` section in the response file.
- If Gemini rejects a Copilot response, move the file into a `rejected/` subfolder under the relevant week in `responses/` and issue a revised prompt (`-v2`).

## ADRs & Traceability
- All architecture-relevant changes MUST be recorded as ADRs under `docs/00_meta/adr/`.
  - **Timing:** Draft ADRs *before* major implementation begins (e.g., choosing DAG vs Chain).
  - **Pivots:** Document mid-code discoveries (e.g., an incompatible .NET 10 library) via a new or superseded ADR.
  - **Constraints:** Document reactive architectural choices (e.g., indexing strategy shifts after a failed test).
- Update `docs/00_meta/Traceability.md` at end-of-week checkpoints with high-level progress and links to ADRs and plans.

## Session Logging
- `session-state.md` contains machine-readable fields (current week, date, status, Last Daily Log) and is authoritative for AI session bootstrap. Do not append historical logs to it.

### The Archive-First Principle
At the end of a session, the daily log (`.../logs/weekly/wXX/YYYY-MM-DD.md`) MUST be created or updated with the session's completed work *before* the live `session-state.md` is updated for the next session's handover. This ensures no work is lost if a session is unexpectedly terminated.

### Append-Only Discipline for Daily Logs
If multiple distinct milestones are achieved within the same day's session, the `YYYY-MM-DD.md` file for that day MUST be appended to, not overwritten. Each new entry MUST use a "Delta Update Block" format as follows:

```
---
### UPDATE-01

**New Key Decision(s):**
- [Decision text]

**New Completed Task(s):**
- [x] [Task text]

**Revised AI Handover Context:**
[A brief, updated handover statement]
```

The full daily log template is used only for the first entry of the day. All subsequent updates must use the Delta Update Block format above. This ensures logs remain deterministic, concise, and easy for agents to parse and generate.

## Review & Maintenance
- Keep this policy small and review it when workflow or tooling changes. Update the `version` header when making breaking changes to protocol.
