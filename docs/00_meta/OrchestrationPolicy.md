---
title: Orchestration Policy
version: 1.0
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
- Update `docs/00_meta/Traceability.md` at end-of-week checkpoints with high-level progress and links to ADRs and plans.

## Session Logging
- `session-state.md` contains machine-readable fields (current week, date, status, Last Daily Log) and is authoritative for AI session bootstrap. Do not append historical logs to it.

## Review & Maintenance
- Keep this policy small and review it when workflow or tooling changes. Update the `version` header when making breaking changes to protocol.
