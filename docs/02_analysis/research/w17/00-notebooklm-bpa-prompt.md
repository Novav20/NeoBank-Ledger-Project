# NotebookLM Extraction Prompts: ISO 20022 & MiFID II (Granular)

Use these prompts to extract requirements from the specific PDF groups to respect page limits.

## Ledger Context
We are building a high-integrity B2B ledger, so extract only the constraints that affect production architecture, domain modeling, compliance, auditability, settlement, and transport.

Treat the standards through the lens of this system vocabulary:
- Gateway: authenticated command ingress and normalization.
- Ordering Service / Sequencer: global sequence assignment and deterministic ordering.
- Validation Shards: conflict checks, shard-local consensus, and acceptance proof.
- Balance Projection: materialized read model derived from validated events.
- Audit Vault: immutable evidence trail and oversight material.

When the standards describe optional capabilities, translate them into one of these outcomes:
- Mandatory ledger requirement.
- Recommended control worth tracking as an ADR or NFR.
- Out of scope for the current ledger baseline.

Prefer requirements that can be turned into domain entities, acceptance criteria, NFRs, or architecture decisions. Focus especially on data precision, message structure, transport integrity, audit metadata, cross-shard atomicity, privacy controls, and settlement-finality implications.

---

## Prompt 1: ISO 20022 Metamodel (Part 1 Only - 162 pages)
**Goal**: Define the fundamental "Grammar" of the Ledger.

> **Task**: From ISO 20022 Part 1, extract the **Core Metamodel Constraints**.
> **Focus**:
> 1. Identify the mandatory **Data Types** and **Business Element** definitions that any compliant ledger must support.
> 2. Extract the rules for **Message Definition** and **Component Relationships**.
> **Output**: A table of fundamental data constraints for our Domain Model, emphasizing what the ledger must store or validate at the Gateway, Sequencer, Shard, or Audit Vault layers.

---

## Prompt 2: UML & Modelling (Parts 2 & 3 - 114 pages)
**Goal**: Define the "Blueprint" logic.

> **Task**: From ISO 20022 Parts 2 and 3, extract the **Modelling Patterns**.
> **Focus**:
> 1. How are **Aggregates** (like Account and Balance) structured in the UML profile?
> 2. What are the mandatory relationships between a **Business Component** and its **Attributes**?
> **Output**: A list of Entity relationship rules for our Domain Model Diagram, aligned to the ledger's eventual ERD and the Balance Projection materialized view.

---

## Prompt 3: Serialization (Parts 4 & 8 - 66 pages)
**Goal**: Define the "Storage" constraints.

> **Task**: From ISO 20022 Parts 4 and 8, extract the **Encoding Requirements**.
> **Focus**:
> 1. What are the mandatory rules for **XML Schema (XSD)** or **ASN.1** that affect data precision or string lengths?
> 2. Identify any constraints that dictate how we must store "Amounts" and "Dates."
> **Output**: A list of physical data type requirements for our ERD, including any constraints that affect precision, truncation risk, timestamp fidelity, or auditability.

---

## Prompt 4: Process & Transport (Parts 5, 6, 7 - 74 pages)
**Goal**: Define the "Technical Physics."

> **Task**: From ISO 20022 Parts 5, 6, and 7, extract the **Business Process & Transport Rules**.
> **Focus**:
> 1. **Part 5**: What are the "Reverse Engineering" constraints for a Credit Transfer (pacs.008)?
> 2. **Part 6**: What are the mandatory **Transport Characteristics** (e.g., Acknowledgement, Integrity, Non-repudiation)?
> **Output**: A list of Non-Functional Requirements (NFRs) for our Gateway and Sequencer, especially anything that affects transport integrity, acknowledgements, provenance, or finality.

---

## Prompt 5: MiFID II Transaction Reporting (148 pages)
**Goal**: Define the "Legal Metadata."

> **Task**: From the MiFID II document, extract the **Technical Reporting Requirements**.
> **Focus**:
> 1. **Timestamp Precision (RTS 25)**: Extract the exact 1ms vs 1μs rules.
> 2. **Metadata Fields**: Identify mandatory fields for LEI (Legal Entity Identifier), UTI (Unique Transaction Identifier), and ISIN.
> 3. **Audit Trail**: What specific data must be preserved to prove "Pre-trade" vs "Post-trade" status?
> **Output**: A Compliance Matrix table of mandatory database fields, highlighting what must be persisted in the Audit Vault or attached to transaction events for regulator-facing evidence.
