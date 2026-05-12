---
Status: Proposed
Date: 2026-05-04
Deciders: Gemini CLI / Juan David
Supersedes: None
---

# ADR-007: Static Mapping over Third-Party Libraries

## Context
In Clean Architecture, we must map Rich Domain Entities (e.g., `Account`) to Persistence DTOs (e.g., `AccountDto`) within the Infrastructure Layer to decouple business logic from storage schemas. Historically, libraries like AutoMapper were used to automate this process. However, recent licensing changes to popular mapping libraries (circa 2026), combined with performance and maintainability concerns, require us to formalize our mapping strategy.

## Decision
We will avoid third-party mapping libraries (like AutoMapper) and instead implement **Manual Static Mappers (Extension Methods)**.

### Implementation Details
- Mappers will be pure, static C# classes (e.g., `AccountMapper`).
- Mapping will be implemented as extension methods: `public static AccountDto ToDto(this Account entity)`.
- Value Objects will be flattened explicitly during the `ToDto` mapping and reconstructed during the `ToEntity` mapping.

## Consequences

### Positive
- **Zero Overhead**: Manual mapping avoids the runtime Reflection overhead typical of dynamic mappers, which is crucial for a high-throughput ledger.
- **Compile-Time Safety**: If a property is renamed or its type changes in the Domain Entity, the compiler will immediately flag the broken mapper. "Magic" mapping libraries often fail silently at runtime.
- **Supply Chain Security**: Removing unnecessary third-party dependencies reduces licensing risks and potential vulnerabilities.

### Negative
- **Boilerplate**: Developers must write more code manually when introducing new entities.
- **Maintenance**: Adding a new property to an entity requires a manual update to the corresponding mapper.

## Grounding
- **Orchestration Policy**: Minimizing external dependencies reduces supply-chain risk and licensing overhead in enterprise software.
- **Performance**: High-frequency financial trading systems prioritize explicit, compile-time-safe execution paths over reflective abstractions.
