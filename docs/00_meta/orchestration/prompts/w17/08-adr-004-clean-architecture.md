---
status: pending
issued_by: Gemini CLI
issued_date: 2026-04-24
response_ref: docs/00_meta/orchestration/responses/w17/08-adr-004-clean-architecture.md
---
# Task: ADR-004 Generation (Adoption of Clean Architecture)

## Task Context
We are finalizing the high-level architecture decisions for the NeoBank Ledger. We need to formalize and defend the choice of the primary architectural pattern (Clean Architecture) before the design freeze and implementation phase.

## Objective
Create `docs/03_architecture/adr/ADR-004-Adoption-of-Clean-Architecture.md` using the standard project template (`docs/00_meta/orchestration/templates/adr-template.md`).

## Specific Instructions
1.  **Decision**: Adopt a **Clean Architecture** (Onion Architecture) pattern for the implementation.
2.  **Rationale**:
    - **Isolation of Core Logic**: Isolate the "Financial Physics" (Double-entry, ISO 20022 validation, PBFT consensus) from external infrastructure (LevelDB, APIs, gRPC).
    - **Testability**: Enable 100% unit test coverage for the Domain layer without infrastructure dependencies.
    - **Maintainability**: Clear separation of concerns between business rules and technical delivery.
3.  **Alternatives Considered**:
    - **Vertical Slice Architecture**: Rejected because the project is "Logic-heavy" (shared rules across all flows) rather than "Feature-heavy."
    - **Monolithic/Procedural (Transaction Script)**: Rejected as unmaintainable for complex B2B compliance.
4.  **Consequences**:
    - **Positive**: Long-term sustainability; logic remains platform-agnostic.
    - **Negative**: Increased initial boilerplate (Interfaces, DTO mapping).

## Constraints & Requirements
- Status: **Accepted**.
- Date: 2026-04-24.
- Deciders: Gemini CLI / Juan David.
- References: Robert C. Martin (Clean Architecture), Vaughn Vernon (Domain-Driven Design).
