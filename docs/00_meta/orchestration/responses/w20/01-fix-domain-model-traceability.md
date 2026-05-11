# Domain Model Physical Traceability Fix

## Executive Summary
I synchronized the Physical Column Traceability table in [Domain-Entity-Model.md](../../../../03_architecture/Domain-Entity-Model.md) with the ERD and Class Diagram. The three explicitly requested `TRANSACTIONS` columns were restored, and the traceability table was expanded to cover the rest of the physical schema so the document is now exhaustive.

## Analysis / Findings
- The archived drafts in `docs/00_meta/archive/architecture_drafts/` confirmed the source grounding for `TRANSACTIONS.end_to_end_id`, `TRANSACTIONS.message_definition_id`, and `TRANSACTIONS.message_function`.
- The original traceability table covered only a subset of the ERD columns. Several physical identifiers, timestamps, routing fields, and audit/rejection fields were undocumented.
- The updated table now includes the full physical set for `PARTIES`, `PARTY_ELIGIBLE_INSTRUMENTS`, `ACCOUNTS`, `TRANSACTIONS`, `EVENTS`, `TRANSACTION_ENTRIES`, `BALANCES`, `AUDIT_BLOCKS`, and `REJECTION_RECORDS`.

## Technical Impact
- `TRANSACTIONS.end_to_end_id` now traces to ISO 20022 identifier semantics and end-to-end business correlation.
- `TRANSACTIONS.message_definition_id` now traces to ISO 20022 message definition metadata.
- `TRANSACTIONS.message_function` now traces to ISO 20022 message function rules and the controlled values `NEWM` / `CANC`.
- The rest of the traceability table now documents the physical columns required by the ERD, including the surrogate keys and routing fields that were previously omitted.

## Deliverable
- Updated [Domain-Entity-Model.md](../../../../03_architecture/Domain-Entity-Model.md)

## Outcome
The domain model documentation now matches the physical schema much more closely and the missing traceability rows have been restored in the existing Markdown table style.