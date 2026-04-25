## Executive Summary
ADR-004 has been accepted to formalize Clean Architecture as the primary implementation pattern for the NeoBank Ledger. The decision keeps the core ledger rules isolated from persistence and transport concerns while preserving testability and long-term maintainability.

## Analysis / Findings
- The project is logic-heavy, not feature-heavy, so a vertical-slice layout would fragment shared ledger rules and duplicate business policy.
- A transaction-script style monolith would not scale well for the project’s compliance, auditability, and invariants-driven domain.
- Clean Architecture fits the current design freeze posture because the domain model is already stabilized and the next step is implementation boundary definition.
- The main trade-off is additional initial boilerplate for interfaces, adapters, and DTO mapping, but that cost is acceptable for this domain.

## Proposed Plan / Solution
1. Keep the Domain and Application layers independent from infrastructure.
2. Push persistence, API, and gRPC concerns into outer adapters.
3. Keep the financial physics, ISO 20022 validation, and PBFT-related policy inside the core.
4. Use dependency inversion so test suites can exercise the ledger rules without external services.

## Technical Impact
- **Affected Files:**
  - `docs/03_architecture/adr/ADR-004-Adoption-of-Clean-Architecture.md`
- **New Dependencies:** None
- **Risk Level:** Low

## Deliverables / Snippets
- Accepted ADR-004 documenting Clean Architecture as the implementation pattern.
- Alternatives considered and rejected: Vertical Slice Architecture, Monolithic / Procedural Transaction Script.
- Testability and maintainability rationale recorded for Sprint 01 readiness.
