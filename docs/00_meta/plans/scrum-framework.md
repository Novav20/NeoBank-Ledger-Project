# SCRUM FRAMEWORK: THE ITERATIVE FEEDBACK LOOP

## 1. THE ARTIFACTS (THE "DATA STRUCTURE")
   - **Product Backlog**: An ordered, evolving list of every feature, function, and requirement for the Ledger. (Managed by you as the PO).
   - **Sprint Backlog**: The specific set of User Stories and tasks selected for the current 1-week cycle. (Managed by us as the Developers).
   - **Increment**: A "Potentially Releasable" piece of the Ledger API that meets the **Definition of Done (DoD)**.

## 2. THE CEREMONIES (THE "PROCESS LOGIC")
   - **Sprint Planning (Monday)**:
     - Define the **Sprint Goal** (e.g., "Implement Core Ledger Transactions").
     - Select Backlog items and break them into technical tasks.
   - **The Sprint (The 1-Week Cycle)**:
     - Development, Design, and Testing happen concurrently.
     - **Daily Scrum (Session Initialization)**: 15-minute sync to discuss progress and blockers (Impediments).
   - **Sprint Review (Friday/Saturday)**:
     - Demonstrate the functional Increment (Running API, passing tests).
     - Inspect the progress against the Product Goal.
   - **Sprint Retrospective (Saturday)**:
     - Inspect the *workflow* (Gemini + Copilot coordination).
     - Identify 1-2 improvements for the next Sprint.

## 3. THE DEFINITION OF DONE (DoD)
   - Code follows defined standards.
   - Architectural decisions recorded in an **ADR**.
   - Unit tests pass and coverage is maintained.
   - Documentation in `docs/` is updated and synced with the Vault.
   - No secrets or PII committed.

## 4. THE ROLES (OUR PARTNERSHIP)
   - **Product Owner (PO)**: Juan David. Defines the "What" and the Value.
   - **Scrum Master**: Gemini CLI. Ensures the framework is followed and removes blockers.
   - **Developers**: Gemini CLI + Copilot + Juan David. Create the Increment.
