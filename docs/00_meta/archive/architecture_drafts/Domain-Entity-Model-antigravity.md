# Domain Model & ERD Synthesis (ISO 20022 & MiFID II)

## 1. Executive Summary
This document synthesizes the rigorous constraints of the **ISO 20022 Metamodel** and **MiFID II Legal Metadata** into a concrete domain and persistence architecture. It establishes the "Technical Physics" for the Ledger, translating conceptual rules (like double-entry precision and temporal fidelity) into concrete C# attributes and database schemas.

---

## 2. Domain Model Diagram (Business Logic)

The Domain Model centers on the aggregates identified in the architectural research: **Transaction** (Intake at Gateway), **Event** (The Immutable Log), and **Balance** (The Materialized Projection).

```mermaid
classDiagram
    %% Aggregates
    class Account {
        <<Aggregate Root>>
        +String AccountID
        +String LEI
        +RegistrationStatus Status
    }
    
    class Balance {
        <<Value Object>>
        +BigInt Amount
        +String Ccy
        +DateTimeOffset LastUpdated
    }
    
    class TransactionIntake {
        <<Command>>
        +String UTI
        +String EndToEndID
        +String SenderLEI
        +String ISIN
        +DateTimeOffset PrecisionTimestamp
        +String MessageDefinition
    }
    
    class LedgerEvent {
        <<Domain Event>>
        +String EventID
        +String UTI
        +String Payload
        +DeliveryAssurance Assurance
    }
    
    class AuditBlock {
        <<Immutable Record>>
        +String BlockID
        +String ChameleonHash
        +String QuorumCert
    }

    %% Relationships
    Account "1" *-- "many" Balance : COMPOSITE (ADR-001)
    TransactionIntake --> LedgerEvent : triggers
    LedgerEvent --> AuditBlock : secured by
    LedgerEvent --> Balance : updates projection
```

### Domain Traceability Notes:
*   **COMPOSITE Aggregation**: As per *ISO 20022-3 (ADR 001)*, a Balance cannot exist independently of an Account.
*   **RegistrationStatus**: Required by *ISO 20022-1 (RepositoryConcept)* to govern the lifecycle of entities (e.g., REGISTERED vs OBSOLETE).

---

## 3. Entity-Relationship Diagram (Storage Structure)

The ERD translates the business logic into deterministic database schema constraints, ensuring precision loss and schema corruption are structurally impossible.

```mermaid
erDiagram
    ACCOUNT ||--o{ BALANCE : "owns (COMPOSITE)"
    ACCOUNT {
        VARCHAR(35) AccountID PK "Source: ISO 20022 IdentifierSet"
        VARCHAR(20) LEI "Source: MiFID II Art 10 (Mandatory for participants)"
        VARCHAR(20) Status "Source: ISO 20022 RegistrationStatus"
    }
    
    BALANCE {
        VARCHAR(35) AccountID FK
        CHAR(3) Ccy "Source: ISO 20022-4 (CurrencyCode NOT NULL)"
        BIGINT Amount "Source: Prompt Override (Smallest currency unit, replacing DECIMAL)"
        TIMESTAMP_TZ LastUpdated "Source: MiFID II RTS 25"
    }
    
    TRANSACTION_INTAKE {
        VARCHAR(52) UTI PK "Source: MiFID II Art 16.7 (Lifecycle Tracking)"
        VARCHAR(35) EndToEndID "Source: ISO 20022 IdentifierSet"
        VARCHAR(20) LEI "Source: MiFID II Art 10"
        VARCHAR(12) ISIN "Source: MiFID II Recital 71"
        TIMESTAMP_TZ PrecisionTimestamp "Source: MiFID II RTS 25 (1ms/1us precision)"
        VARCHAR(35) MessageDefinition "Source: ISO 20022-1 (Message Intent)"
    }
    
    LEDGER_EVENT {
        VARCHAR(35) EventID PK 
        VARCHAR(52) UTI FK "Source: Trace Rule to Transaction"
        VARCHAR(20) Intent "Source: ISO 20022-6 (e.g., NEWM, CANC)"
        BLOB RawPayload "Source: ISO 20022-4 (UTF-8 XML/ASN.1 Storage)"
    }
    
    AUDIT_BLOCK {
        VARCHAR(64) BlockID PK
        VARCHAR(35) EventID FK
        VARCHAR(256) ChameleonHash "Source: Redactable Ledger ADR"
        VARCHAR(512) QuorumCert "Source: BFT Finality (QC)"
        TIMESTAMP_TZ SettledAt "Source: MiFID II RTS 25"
    }
```

---

## 4. Compliance Attributes & Data Types (C# & DB)

### 4.1 Monetary Precision (The `BigInt` Mandate)
*   **Type**: `long` (C#) / `BIGINT` (Database)
*   **Constraint**: Financial amounts MUST be tracked in the smallest possible currency unit (e.g., cents or fractional cents) to entirely eliminate floating-point rounding errors. This effectively realizes the *ISO 20022-4 DECIMAL(18,5)* scale through integer arithmetic.

### 4.2 Temporal Fidelity (MiFID II RTS 25)
*   **Type**: `DateTimeOffset` (C#) / `TIMESTAMP WITH TIME ZONE` (Database)
*   **Constraint**: All timestamps MUST include UTC offset (`Z`) and maintain 7 decimal places of precision, capturing the 1ms (standard) or 1μs (HFT) requirements defined by MiFID II Art 17.

### 4.3 Identifier Constraints
| Attribute | Length | C# Type | Database Type | Source Constraint |
| :--- | :--- | :--- | :--- | :--- |
| **UTI** | 52 | `string` | `VARCHAR(52)` | MiFID II Art 16.7 (Unique Transaction Identifier). |
| **LEI** | 20 | `string` | `VARCHAR(20)` | MiFID II Art 10 (Legal Entity Identifier). |
| **ISIN** | 12 | `string` | `VARCHAR(12)` | MiFID II Recital 71 (Instrument Identification). |
| **EndToEndID** | 35 | `string` | `VARCHAR(35)` | ISO 20022 IdentifierSet (`Max35Text`). |
| **Currency** | 3 | `string` | `CHAR(3)` | ISO 20022-4 (Ccy must be NOT NULL). |

### 4.4 Audit & Consensus Layer
*   **ChameleonHash**: A specialized hash structure stored as `VARCHAR(256)` in the `AuditBlock` to satisfy the GDPR "Right to Erasure" vs Immutability conflict (ADR requirement).
*   **QuorumCert (QC)**: A cryptographic proof of consensus finality stored as `VARCHAR(512)` to ensure audited, deterministic finality.
