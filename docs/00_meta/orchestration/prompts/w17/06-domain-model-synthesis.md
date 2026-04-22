---
status: pending
issued_by: Gemini CLI
issued_date: 2026-04-22
response_ref: docs/00_meta/orchestration/responses/w17/06-domain-model-synthesis.md
---
# Task: Domain Model & ERD Synthesis (ISO 20022 & MiFID II)

## Task Context
We have completed a granular extraction of technical requirements from the ISO 20022 Metamodel (Parts 1-8) and MiFID II laws. We now need to synthesize these findings into a concrete **Domain Model Diagram** and **Entity-Relationship Diagram (ERD)** to guide our C# implementation.

## Objectives
1.  **Map Domain Entities**: Translate ISO 20022 "Business Components" (from extractions) into our system entities (Account, Transaction, Entry, AuditBlock).
2.  **Define Compliance Attributes**: Incorporate MiFID II mandatory fields (UTI, LEI, 1ms Timestamp) and ISO 20022 constraints (CurrencyAmount, Data Types) into the schema.
3.  **Create Visualization**: Generate Mermaid diagrams for both the Domain Model (Business logic) and the ERD (Storage structure).

## Source Material
Analyze the following files in `docs/02_analysis/research/w17/ISO-20022-n-MiFID-II-Extractions/`:
- `Part-1-ISO-20022-Metamodel.md`
- `Part-2-3-UML-and-Modelling.md`
- `Part-4-8-Serialization.md`
- `Part-5-6-7-Process-n-Transport.md`
- `MiFID-II-Transaction-Reporting.md`

## Specific Instructions

### 1. Domain Model Diagram (Mermaid erDiagram or classDiagram)
- Focus on the **Aggregates**: Transaction (Intake), Event (The Log), and Balance (The Projection).
- Show the relationships based on ISO 20022 Part 1/2 metamodel rules.
- Persona-based attributes (Fintech Partner, Sponsor Bank, Auditor).

### 2. Entity-Relationship Diagram (ERD)
- Define physical columns and data types.
- **Precision**: Money MUST be `BigInt` (smallest currency unit).
- **Time**: Timestamps MUST be `DateTimeOffset` with 7 decimal places (MiFID II RTS 25).
- **Compliance IDs**: `UTI` (52 chars), `LEI` (20 chars), `EndToEndID` (ISO 20022).
- **Audit Layer**: Include `ChameleonHash` and `QuorumCert` in the AuditBlock table.

### 3. Traceability
- Every entity and major attribute should have a small reference note (e.g., "Source: ISO 20022-1" or "Source: MiFID II RTS 25").

## Expected Output
Create `docs/03_architecture/Domain-Entity-Model.md` containing the synthesis, diagrams, and compliance attribute list.
