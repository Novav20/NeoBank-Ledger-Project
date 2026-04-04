# GitHub Copilot: Global Repository Invariants

## Strategic Alignment & Architecture
- **Senior Lead:** Gemini CLI (Local Architect). Always prioritize the architectural patterns defined in `docs/00_meta/adr/`.
- **Documentation First:** If a task requires research or context, look into the `docs/` folder first to understand existing Business Rules and ADRs. If context is missing, proceed using .NET 10 best practices and Clean Architecture.

## Strict Technical Constraints
- **Stack:** .NET 10, EF Core 10, SQL Server.
- **Standards:**
  - Always use **Primary Constructors** for Dependency Injection.
  - Prefer `ExecuteUpdate` and `ExecuteDelete` for bulk database operations where performance is critical.
  - Always use `System.Text.Json` for serialization.
  - Never use emojis in code or internal documentation.
  - Language: English.
- **Architecture:** Clean Architecture with a focus on high-transactionality and auditability (Immutability where possible).

## Operational Protocols
- Operational rules (session bootstrap, naming conventions, ADR requirements, and traceability) are maintained in a single source of truth: `docs/00_meta/OrchestrationPolicy.md`. Read that file at session start and follow its instructions.
- The `docs/00_meta/orchestration/logs/session-state.md` file remains the live snapshot for bootstrapping.
