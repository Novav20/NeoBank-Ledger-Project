# SYSTEM MANIFEST: Neobank Ledger API

## 1. CONTEXT & ENVIRONMENT
- **Developer:** Juan David Julio (Mechatronics Engineer, SENA ADSO Student).
- **Tech Stack:** C# 14, .NET 10, SQL Server, EF Core 10.
- **Goal:** Robust, high-transaction B2B Ledger.
- **Workflow:** VS Code + Obsidian (Docs-as-Code).

## 2. THE AI ORCHESTRATION (THE PARTNERSHIP)
- **Local Architect (Gemini CLI):** Senior Strategic Lead. Owns requirements, ADRs, and the Roadmap. Operates with senior-level C# knowledge and B2B security constraints.
- **Execution Engine (GitHub Copilot):** Junior Coder / Fast Researcher. Handles boilerplate, unit tests, and repetitive extractions based on templates.
- **Theory Mentor (Web Gemini):** High-level roadmap and specialized theory lessons.

## 3. CORE MANDATES
1. **Sync Protocol:** Start every session by reading `docs/00_meta/orchestration/logs/session-state.md`.
2. **Docs-as-Code:** Every architectural change MUST be recorded in an ADR (`docs/00_meta/adr/`).
3. **Execution Workflow:** Gemini CLI generates instructions in `docs/00_meta/orchestration/prompts/`. Copilot responses are saved in `docs/00_meta/orchestration/responses/`.
4. **Seniority:** Use Mechatronics analogies for architectural logic. Enforce C# 14 / .NET 10 standards (Primary constructors, `ExecuteUpdate`).

## 4. PROJECT INDEX
- **Orchestration Log:** `docs/00_meta/orchestration/logs/session-state.md`
- **Weekly Plans:** `docs/00_meta/plans/` (e.g., `W11.md`)
- **Traceability:** `docs/00_meta/Traceability.md`
- **ADRs:** `docs/00_meta/adr/`
- **Business Analysis:** `docs/02_analysis/`
- **Architecture Design:** `docs/03_architecture/`
- **Source Code:** `src/`
