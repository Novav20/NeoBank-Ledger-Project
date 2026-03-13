# Contributing and Maintainer Workflow

This project follows an ADR-first, docs-as-code workflow.

## Core Principles
- Make architectural decisions explicit in `docs/00_meta/adr/` before large implementation changes.
- Keep requirements, architecture, and execution plans synchronized.
- Favor high-transactionality and auditability concerns in all backend changes.

## Internal Orchestration (Maintainers)
- Session bootstrap state lives in `docs/00_meta/orchestration/logs/session-state.md`.
- Prompt and response artifacts are organized in:
  - `docs/00_meta/orchestration/prompts/`
  - `docs/00_meta/orchestration/responses/`
- Week naming convention is lowercase `wNN`.

## Governance References
- `GEMINI.md`
- `.github/copilot-instructions.md`
- `docs/00_meta/DefinitionOfDone.md`
- `docs/00_meta/NamingConventions.md`
- `docs/00_meta/Traceability.md`

## Notes
- Local-only archives are under `docs/00_meta/archive/` and may be gitignored.
- Some working lesson notes under `docs/01_lessons/w0/` are currently configured as local-only.