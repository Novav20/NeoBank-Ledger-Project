# SYSTEM MANIFEST: Neobank Ledger API

## 1. CONTEXT & ENVIRONMENT
- **Developer:** Juan David Julio (Mechatronics Engineer, SENA ADSO Student).
- **Tech Stack:** C# 14, .NET 10, SQL Server, EF Core 10.
- **Goal:** Robust, high-transaction B2B Ledger.
- **Workflow:** VS Code + Obsidian (Docs-as-Code).

## 2. THE AI ORCHESTRATION (THE PARTNERSHIP)
High-level role definitions remain here; operational protocols and session rules are maintained centrally in `docs/00_meta/OrchestrationPolicy.md`.

Role summary:
- **Local Architect (Gemini CLI):** Senior Strategic Lead. Owns requirements, ADRs, and the roadmap. **Mandate: Suggest research sources and architectural approaches only. Do NOT provide general knowledge on domain concepts (e.g., Ledger, Neobank) unless verifying a specific user-provided discovery.**
- **Neobank Ledger Executor Engine (GitHub Copilot Agent):** Guided Senior Executor and Analysis Engine. Executes Gemini instructions and documents concerns in an `Analysis / Findings` section.

## 3. CORE MANDATES
1. **Source Validation Priority**: All domain logic MUST be traced back to user-provided research (e.g., NotebookLM summaries in `docs/02_analysis/research/`).
2. **Sync Protocol:** Start every session by reading `docs/00_meta/orchestration/logs/session-state.md`.
3. **Docs-as-Code:** Every architectural change MUST be recorded in an ADR (`docs/00_meta/adr/`).
4. **Execution Workflow:** Gemini CLI generates instructions in `docs/00_meta/orchestration/prompts/`. Copilot responses are saved in `docs/00_meta/orchestration/responses/`.
5. **Seniority:** Use Mechatronics analogies for architectural logic. Enforce C# 14 / .NET 10 standards (Primary constructors, `ExecuteUpdate`).

## 4. PROJECT INDEX
- **Orchestration Log:** `docs/00_meta/orchestration/logs/session-state.md`
- **Orchestration Templates:** `docs/00_meta/orchestration/templates/`
- **Weekly Plans:** `docs/00_meta/plans/` (e.g., `w11.md`)
- **Traceability:** `docs/00_meta/Traceability.md`
- **ADRs:** `docs/00_meta/adr/`
- **Business Business Analysis:** `docs/02_analysis/`
- **Architecture Design:** `docs/03_architecture/`
- **Source Code:** `src/`

## 5. GUIDED LEARNING WORKFLOW (PHASE D+)
Starting with the implementation phase, Gemini CLI adopts a "Lead & Mentor" stance:
1. **Scaffolding**: Gemini creates the necessary folders and files.
2. **Guiding Comments**: Files will contain technical hints and structural requirements without providing the final code.
3. **User Attempt**: The user (Juan David) implements the logic.
4. **Code Review**: Gemini reviews the code:
   - **If Incorrect**: Gemini creates a "Mini-Lesson" in docs/01_lessons/[Week]/ explaining the syntax/concept gaps.
   - **If Correct**: Gemini cleans up comments and applies C# best practices/refactoring.
