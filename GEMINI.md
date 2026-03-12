# SYSTEM MANIFEST: Neobank Ledger API

## 1. CONTEXT & ENVIRONMENT
- **Developer:** Juan David Julio (Mechatronics Engineer, SENA ADSO Student).
- **Tech Stack:** C# 14, .NET 10, SQL Server, EF Core 10.
- **Goal:** Robust, high-transaction B2B Ledger.
- **Workflow:** VS Code + Obsidian (Docs-as-Code).

## 2. THE AI ORCHESTRATION (THE PARTNERSHIP)
- **Local Architect (Gemini CLI):** Senior Strategic Lead. Owns requirements, ADRs, and the Roadmap. Operates with senior-level C# knowledge and B2B security constraints.
- **Execution Engine (GitHub Copilot):** Guided Senior Executor and Analysis Engine. Executes architectural designs defined by Gemini CLI. Performs large-scale text analysis, synthesis, and research when context load exceeds Gemini CLI capacity. Handles code generation, boilerplate, and unit tests. May flag (never override) better alternatives in the "Analysis / Findings" section of its response.
- **External Research Tools (Optional):** Web Gemini, Microsoft Learn, and similar sources are optional references used by the developer. They are not formal workflow agents and do not write directly to orchestration paths.

## 3. CORE MANDATES
1. **Sync Protocol:** Start every session by reading `docs/00_meta/orchestration/logs/session-state.md`. That file contains a `Last Daily Log` field pointing to the most recent daily log — read it as well for full session context.
  - `session-state.md` is the live snapshot and is overwritten (not appended) at the start of each new session by Gemini CLI.
  - At the end of each week, copy the final state to `docs/00_meta/orchestration/logs/weekly/WXX/YYYY-MM-DD.md`.
  - Weekly logs are the archive; `session-state.md` is always the current context.
2. **Docs-as-Code:** Every architectural change MUST be recorded in an ADR (`docs/00_meta/adr/`).
3. **Execution Workflow:** Gemini CLI generates instructions in `docs/00_meta/orchestration/prompts/`. Copilot responses are saved in `docs/00_meta/orchestration/responses/`.
4. **Seniority:** Use Mechatronics analogies for architectural logic. Enforce C# 14 / .NET 10 standards (Primary constructors, `ExecuteUpdate`).
5. **Traceability Scope:**
  - `docs/00_meta/Traceability.md` tracks high-level weekly/milestone progress and should be updated at end-of-week checkpoints.
  - `docs/00_meta/orchestration/logs/session-state.md` tracks machine-readable live session context for active work.

## 4. PROJECT INDEX
- **Orchestration Log:** `docs/00_meta/orchestration/logs/session-state.md`
- **Orchestration Templates:** `docs/00_meta/orchestration/templates/`
- **Weekly Plans:** `docs/00_meta/plans/` (e.g., `W11.md`)
- **Traceability:** `docs/00_meta/Traceability.md`
- **ADRs:** `docs/00_meta/adr/`
- **Business Analysis:** `docs/02_analysis/`
- **Architecture Design:** `docs/03_architecture/`
- **Source Code:** `src/`

---

## Escalation & Rejection Protocol

- **Copilot Flags a Prompt Flaw:**
  - Copilot executes the prompt as written, but documents any concerns or better alternatives in the "Analysis / Findings" section of its response.
- **Gemini Rejects a Copilot Response:**
  - The rejected response file is moved to a `rejected/` subfolder under the relevant week in `responses/`.
  - Gemini issues a new prompt (with a `-v2` or incremented suffix) for the same task.
- **ADRs Become Outdated:**
  - Superseded ADRs are not deleted. Instead, a new ADR is created with a `Supersedes: ADR-XXX` field, and the old ADR's status is set to `Superseded by ADR-XXX`.
