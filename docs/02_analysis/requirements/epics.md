---
type: epic-definition
version: 1.0
last_edited: 2026-04-15
status: draft
source:
  - docs/02_analysis/bpa/BPA_Report.md#L24
  - docs/02_analysis/bpa/BPA_Report.md#L88
  - docs/02_analysis/bpa/BPA_Report.md#L131
---

# Epic Definitions

Epic IDs use stable 4-letter mnemonic codes instead of numeric sequencing to keep the backlog concise and easier to reference.

| ID   | Name                              | BPA Reference                                                                                                                                                                                                                                                                        | Business Value                                                                                                                            |
| ---- | --------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------- |
| TXEN | Transaction Engine (EOV)          | [[BPA_Report#1.3 Stakeholders & Ecosystem\|BPA 1.3 Stakeholders & Ecosystem]]; [[BPA_Report#5.2 To-Be Flow (Event Sourcing + Shards + Fabric-like EOV)\|BPA 5.2 To-Be Flow]]                                                                                                          | Gives fintech partners a deterministic intake-to-validation pipeline that turns external intent into ledger-ready transactions.           |
| PROJ | Balance & State Projection        | [[BPA_Report#5.2 To-Be Flow (Event Sourcing + Shards + Fabric-like EOV)\|BPA 5.2 To-Be Flow]]; [[BPA_Report#4.1 Foundational Principles (Core Business Rules)\|BPA 4.1 Foundational Principles]]                                                                                     | Produces trusted world-state views so partners and sponsor banks can query balances without scanning the event log.                       |
| AUDT | Integrity & Audit Vault           | [[BPA_Report#1.3 Stakeholders & Ecosystem\|BPA 1.3 Stakeholders & Ecosystem]]; [[BPA_Report#4.1 Foundational Principles (Core Business Rules)\|BPA 4.1 Foundational Principles]]; [[BPA_Report#5.2 To-Be Flow (Event Sourcing + Shards + Fabric-like EOV)\|BPA 5.2 Settle & Observe]] | Preserves immutable proof material for auditors and regulators, enabling non-repudiation and continuous oversight.                        |
| RECN | External Liquidity Reconciliation | [[BPA_Report#1.3 Stakeholders & Ecosystem\|BPA 1.3 Stakeholders & Ecosystem]]; [[BPA_Report#5.2 To-Be Flow (Event Sourcing + Shards + Fabric-like EOV)\|BPA 5.2 Settle & Observe]]                                                                                                    | Keeps internal ledger positions aligned with sponsor-bank liquidity references and surfaces discrepancies for correction.                 |
| OBSV | Operational Observability         | [[BPA_Report#1.3 Stakeholders & Ecosystem\|BPA 1.3 Stakeholders & Ecosystem]]; [[BPA_Report#5.2 To-Be Flow (Event Sourcing + Shards + Fabric-like EOV)\|BPA 5.2 Settle & Observe]]                                                                                                    | Gives operators the health, liveness, and anomaly signals needed to maintain service quality and react before failures affect settlement. |
