# Technical Physics: Production Constraints for High-Integrity B2B Ledger (ISO 20022 Parts 5 & 6)

The Gateway serves as the critical strategic frontier of the ledger ecosystem, acting as the primary normalization point where diverse legacy industry messages are transformed into high-integrity ledger commands. To maintain the "Technical Physics" of the ledger—the immutable laws governing its state and transitions—we must strictly adhere to the reverse engineering methodology of ISO 20022-5. Strict adherence to gap analysis is not a documentation exercise; it is the functional requirement that ensures the business meaning of a financial contract remains intact during the transition from legacy transport into our model-driven architecture.

## 1. Gateway Ingress: Normalization and Reverse Engineering Constraints

The Gateway normalization engine MUST function as a deterministic reverse-engineering processor. Based on ISO 20022-5 Section 5.2.2, the engine shall not merely map fields but resolve semantic gaps where legacy formats obscure business intent.

### Reverse Engineering Workflow Requirements

To comply with the "Main steps in gap analysis" (Figures 1 & 2), the normalization engine MUST meet the following functional requirements:

- **Semantic Resolution (Note 1):** The engine MUST support "Multi-field concatenation" and "Partial field extraction." If a legacy field contains multiple BusinessElements or only a partial element, the engine shall combine or split these into discrete, meaningful BusinessElements before ledger submission.
- **Data Class Distinction (Note 2 & 3):** The engine MUST distinguish between:
    - **Business Meaning:** Core BusinessElements/Components representing financial intent.
    - **Technical Fields:** Infrastructure-specific data (e.g., transport headers) with no business meaning. These SHALL be isolated and are strictly forbidden from affecting the core ledger state.
    - **Multi-functional Indicators:** Fields that define the purpose of a message (e.g., distinguishing an MT 502 "Order to Buy" from a "Cancellation").

### Credit Transfer (pacs.008) Ingress Logic

Acceptance of a `pacs.008` (or legacy equivalent) is predicated on the preservation of "MessagePaths" to ensure semantic integrity.

|   |   |   |
|---|---|---|
|ISO 20022 Message Concept|Gateway Entity Mapping|Architectural Requirement|
|**MessageDefinition**|`Command.Payload.Header`|MUST match specific functionality (e.g., NEWM vs CANC).|
|**MessagePath**|`Command.Payload.Address`|MUST uniquely identify the hierarchy from root to element.|
|**MessageElement**|`Command.Payload.Data`|MUST map to a specific BusinessElement in the DataDictionary.|
|**MessageConstraint**|`Ledger.Invariant`|Legacy formats (e.g., 15d in ISO 15022) MUST map to precision-protected types.|

### Mandatory Data Transformation (Annex A)

To prevent precision loss during ingress, the Gateway SHALL implement the following NFRs based on DataType convergence (Table A.1):

- **Rule DT001 (Precision):** FIX "Price" (float) MUST be converted to ISO 20022 "Amount" using decimal-safe scaling to prevent floating-point errors.
- **Rule DT002 (Identification):** ISO 15022 "Account Number /35x" MUST be mapped to the `AccountNumber_Identifier` type.
- **Rule DT003 (Enumeration):** Legacy codes (e.g., "BASK" for Trade Transaction Type) MUST be mapped via value conversion tables to the ISO 20022 `TradeType_Code`.

The "Message item convergence table" (Section 5.5.2.2.4) serves as our "Technical Physics" for resolving ambiguity. Normalized commands are then handed off to the Ordering Service for global sequencing.

## 2. Ordering Service & Sequencer: Transport Integrity and Determinism

The Sequencer defines the temporal logic of the ledger. "Transport Characteristics" from ISO 20022-6 are core ledger constraints that define the physics of message order and finality.

### Transport Characteristic NFRs

The Sequencer MUST satisfy the following NFRs derived from ISO 20022-6 to maintain ledger integrity:

- **Integrity:** The Sequencer SHALL guarantee message content immutability. Failure of an integrity check MUST result in the immediate rejection of the sequence number and isolation of the message envelope.
- **Acknowledgement:** The system SHALL implement a deterministic receipt mechanism to ensure settlement finality.
- **Non-repudiation:** The system SHALL provide proof of origin and receipt, preventing a sender from denying a sequenced transaction.

### Sequence Assignment & Intent Validation

Per Section 5.2.5, the Sequencer MUST append metadata to every message to maintain an immutable evidence trail.

- **Intent Validation:** The Sequencer SHALL validate the "23G" field (as seen in Annex A.3) to ensure message intent (NEWM vs CANC) aligns with the expected state in the BusinessTransaction flow.
- **Provenance:** Every message SHALL carry its `MessageDefinition` functionality metadata, ensuring the ledger understands the business reason for the event.

### Constraint Translation Table

|   |   |   |
|---|---|---|
|Requirement|Outcome|Architectural Justification|
|**Acknowledgement**|Mandatory Ledger Req.|Required for deterministic settlement finality.|
|**Integrity Checks**|Mandatory Ledger Req.|Rejection/Isolation of envelope to prevent state corruption.|
|**Non-repudiation**|ADR: Audit Requirement|Provides legal certainty and independent verification.|
|**Bi-directional Mapping**|Recommended ADR|Necessary for "Gradual" migration and coexistence periods.|

Once a message is ordered, its position in the "Technical Physics" of the ledger is irrevocable, satisfying the requirements for settlement finality implied by the BusinessTransaction definitions.

## 3. Validation Shards: Business Process & Conflict Logic

Validation Shards execute the "BusinessProcess gap analysis" (Section 5.2.4) logic through shard-local consensus, ensuring cross-shard atomicity.

### Validation Logic Grounding

Shards MUST treat the "Arguments" identified in Section 5.2.4.2 as a strict schema for state transitions.

- **Deterministic Schema:** If a required argument is missing in the normalized command, the transaction is non-deterministic and MUST fail.
- **Pre- and Post-conditions:** Shards SHALL only commit transactions where the ledger state satisfies the pre-conditions of the BusinessProcess and the resulting state aligns with its post-conditions.

### Business Role and Conflict Verification

- **Role Verification (5.2.7):** Shards MUST validate the "functional roles" (e.g., buyer, seller) of the sender against the message content. Transactions from unauthorized roles SHALL be rejected.
- **Functionality Branching:** Shard logic MUST use "MessageDefinition functionality" metadata (Section 5.2.6) to branch validation. For example, an MT 502 handled as a "Buy" vs a "Cancel" requires distinct conflict-check logic to prevent double-spending or unauthorized state updates.

## 4. Balance Projection & Audit Vault: Read Models and Immutable Evidence

Strategic integrity requires the separation of materialized read models (Balance Projection) from the immutable evidence trail (Audit Vault).

### Materialized Read Model Constraints

Based on Section 5.2.6, the Balance Projection engine is strictly limited:

- **Forbidden Data:** The engine SHALL NOT consume any data that lacks a "BusinessElement" mapping in the ISO 20022 DataDictionary.
- **Consistency:** All projections MUST be derived from validated BusinessElements to ensure a pure reflection of the financial model.

### Audit Vault and Non-Repudiation

The Audit Vault acts as the repository for "Convergence documentation" (Section 5.5.2). To satisfy Section 5.5.2.1’s objective of "independent verification," the Vault MUST store:

- **Source Syntax:** The raw legacy string (e.g., ISO 15022) alongside the normalized command.
- **Convergence Metadata:** The `MessagePath`, `Original Item Value`, and the specific `Convergence Rule Identifier` (e.g., DT001) used.
- **Auditability:** This allows for "Reverse Physics" checks, where auditors can replay the transformation logic to verify the current balance projection.

## 5. Technical Physics Summary: NFR & ADR Registry

The architectural posture of this ledger is derived from the methodical rigor of ISO 20022. By treating message transport and reverse engineering as "Technical Physics," we ensure a high-integrity ecosystem.

### Consolidated NFR List

- **Transport Integrity:** The system SHALL reject and isolate any message envelope that fails integrity checks (Part 6).
- **Data Precision:** The system MUST adhere to DataType convergence tables (Annex A) to prevent precision loss.
- **Settlement Finality:** All sequenced messages SHALL be irrevocable once assigned a position in a BusinessTransaction flow.
- **Auditability:** The system SHALL store the full convergence trail, including Rule IDs and original source syntax.

### Core ADR Recommendations

1. **ADR 1: Strict Business-only State Transitions:** Rationale: ISO 20022-5 Section 5.2.2 Note 3 requires isolation of technical fields to prevent semantic drift in the ledger core.
2. **ADR 2: Deterministic Message Intent:** The system SHALL validate the "23G" intent field at the Sequencer level to ensure message flow consistency.
3. **ADR 3: Independent Verification Repository:** The Audit Vault SHALL maintain raw source syntax to allow third-party verification of normalization physics.

Continuous alignment with ISO 20022 Part 7 is mandatory. The system's Data Dictionary service MUST be dynamically updateable to synchronize with Registration Authority updates, ensuring long-term integrity and global interoperability.