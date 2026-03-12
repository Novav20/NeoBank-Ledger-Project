# Definition of Done

## Purpose
This document defines minimum completion criteria for core project artifacts.

## 1. ADR (Architecture Decision Record)
A new ADR is done when:
- Decision context is described (problem and constraints).
- Final decision is explicitly stated.
- Alternatives and trade-offs are documented.
- Consequences/risks are listed.
- Status and date are filled.

## 2. Requirements Document
A requirements artifact is done when:
- Functional requirements are listed and numbered.
- Non-functional requirements are listed and measurable where possible.
- Scope boundaries are explicit (in-scope / out-of-scope).
- Open assumptions are documented.
- Traceability links to related ADRs or plans exist.

## 3. Weekly Plan
A weekly plan is done when:
- SMART objective is defined.
- Daily tasks are listed for the target week.
- Milestone and expected outputs are defined.
- Dependencies/blockers are documented.
- Status field reflects current progress.

## 4. Session Log
A session log is done when:
- Current phase and active week are specified.
- Completed tasks are recorded.
- Pending tasks for next session are listed.
- Handover context is concise and actionable.
- Last daily log reference is present.

## 5. Copilot Prompt/Response Pair
A prompt-response unit is done when:
- Prompt includes context, objectives, and constraints.
- Prompt frontmatter status is updated (`pending` → `executed` or `rejected`).
- Response includes analysis/findings and technical impact.
- Any concerns are logged in "Analysis / Findings".
- Output path is traceable under `docs/00_meta/orchestration/responses/`.
