# Copilot Response: Domain Model & ERD Synthesis (ISO 20022 & MiFID II)
**Location:** docs/00_meta/orchestration/responses/w17/06-domain-model-synthesis.md

---

## Executive Summary
I created [Domain-Entity-Model.md](/home/novillus/Documents/vscode/NeoBank-Ledger-Project/docs/03_architecture/Domain-Entity-Model.md) to synthesize the ISO 20022 and MiFID II extraction notes into the ledger's domain model and ERD. The document maps the core business aggregates to Account, Transaction, Entry, Balance, and AuditBlock, and it captures the physical storage rules for money, time, identifiers, and audit evidence.

## Analysis / Findings
The critical synthesis choice is that the ledger does not persist monetary values as floating point or regular decimal amounts. Even though ISO 20022 describes Amount as a decimal-oriented logical construct, the physical model stores money as `BIGINT` minor units plus a `CHAR(3)` currency code so the system preserves exactness across shards and avoids rounding drift.

The second important synthesis choice is temporal fidelity. MiFID II RTS 25 distinguishes 1ms and 1us reporting precision depending on the trading context, so the physical model uses `DateTimeOffset(7)` to preserve a superset of that precision. Validation rules can still enforce the reporting floor, but storage should not throw away precision before the Audit Vault can retain it.

The resulting model is intentionally pre-code and traceable:
- Transaction is the intake aggregate root.
- Entry is the immutable double-entry line item.
- Balance is a projection, not a competing source of truth.
- AuditBlock is the immutable evidence layer and carries `ChameleonHash` and `QuorumCert`.

## Proposed Plan / Solution
1. Use [Domain-Entity-Model.md](/home/novillus/Documents/vscode/NeoBank-Ledger-Project/docs/03_architecture/Domain-Entity-Model.md) as the domain reference for future entity work.
2. Validate the ERD against the HLD before any C# entity implementation begins.
3. Keep the next design step pre-coding and focused on design-freeze review.

## Technical Impact
- **Affected Files:** `docs/03_architecture/Domain-Entity-Model.md`, `docs/00_meta/orchestration/logs/weekly/w17/2026-04-22.md`, `docs/00_meta/orchestration/logs/session-state.md`, `docs/00_meta/plans/w17.md`
- **New Dependencies:** None
- **Risk Level:** Medium

## Deliverables / Snippets
### Synthesis outcomes
- Account, Transaction, Entry, Balance, and AuditBlock are now modeled with traceable ISO 20022 and MiFID II attributes.
- `amount_minor_units` is modeled as `BIGINT` and paired with `currency_code` to preserve exactness.
- `event_timestamp`, `booked_at`, `as_of`, and `committed_at` are modeled as `DateTimeOffset(7)` / `datetimeoffset(7)` for audit-grade precision.
- `UTI`, `LEI`, and `EndToEndID` are explicitly represented as compliance identifiers.
- `ChameleonHash` and `QuorumCert` are stored in AuditBlock to anchor redactability and deterministic finality.

### Critical note
The physical model is a deliberate ledger adaptation of the logical standards. It preserves the business meaning of ISO 20022 and MiFID II while choosing storage types that are safer for a high-integrity, multi-shard B2B ledger.
