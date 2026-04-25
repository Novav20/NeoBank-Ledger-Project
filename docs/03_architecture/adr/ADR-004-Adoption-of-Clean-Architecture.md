# ADR-004: Adoption of Clean Architecture
**Status:** Accepted
**Date:** 2026-04-24
**Deciders:** Gemini CLI / Juan David
**Supersedes:** None

---

## Context
The NeoBank Ledger is a logic-heavy B2B system. The core implementation must keep the domain rules for double-entry bookkeeping, ISO 20022 validation, PBFT-driven finality, and shard-aware routing isolated from delivery concerns such as LevelDB persistence, API endpoints, and gRPC transport.

Without a strict architectural boundary, infrastructure details would leak into the domain model, test coverage would depend on external systems, and compliance rules would become harder to reason about and evolve. The project needs a pattern that keeps the Financial Physics stable while allowing the outer delivery and persistence mechanisms to change.

## Decision
We will adopt Clean Architecture, also known as Onion Architecture, as the primary implementation pattern. The Domain and Application layers will remain independent of infrastructure concerns, and all dependencies will point inward toward the core business rules.

## Consequences

### Positive
- The financial domain remains platform-agnostic and isolated from storage and transport concerns.
- Domain and application logic can be unit tested without LevelDB, APIs, or gRPC.
- The architecture supports long-term maintainability by separating business rules from technical delivery.
- Infrastructure choices can evolve without forcing changes into the core ledger model.

### Negative / Trade-offs
- Initial implementation will require more boilerplate, including interfaces, adapters, and DTO mapping.
- The team must maintain discipline around dependency direction and boundary placement.

### Risks
- Over-abstraction can slow early delivery if boundaries are drawn too aggressively; mitigate by keeping the core focused on real ledger rules and introducing abstractions only where an outer concern is expected to vary.
- An anemic domain model could appear if business behavior is pushed outward; mitigate by keeping invariants, validation, and policy decisions inside the core.

## Alternatives Considered
| Alternative | Reason Rejected |
|---|---|
| Vertical Slice Architecture | Rejected because this system is logic-heavy and shares core rules across flows, so a feature-first layout would fragment the domain model and duplicate rules. |
| Monolithic / Procedural Transaction Script | Rejected because the project has complex compliance, validation, and auditability requirements that would become difficult to maintain and test. |

## References
- Robert C. Martin, *Clean Architecture*
- Vaughn Vernon, *Domain-Driven Design Distilled*
- [[High-Level-Design]]
- [[ADR-001-GDPR-Compliance]]
- [[ADR-002-Deterministic-Finality-via-PBFT]]
- [[ADR-003-Sharding-Topology-via-MSSP]]
