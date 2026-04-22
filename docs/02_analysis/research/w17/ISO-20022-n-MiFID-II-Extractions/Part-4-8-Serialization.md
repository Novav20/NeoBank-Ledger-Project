# Production Storage Constraints: ISO 20022 Encoding & Data Precision Requirements

## 1. Architectural Alignment and Strategic Context

In the architecture of a high-integrity B2B ledger, the **ISO 20022 Repository** serves as the **Single Source of Truth (SSOT)**. Strategic alignment with **ISO 20022 Part 4 (XML)** and **Part 8 (ASN.1)** is not a discretionary compliance task; it is a fundamental architectural mandate to ensure that the physical storage layer is a high-fidelity representation of the logical financial model.

Per **ISO 20022-4 Section 1**, the transformation from a MessageDefinition to its physical schema is **deterministic**. This implies that the storage layer—encompassing the **Gateway** (ingress) and the **Audit Vault** (storage)—must be auto-generated from the model. As Principal Ledger Architect, I am establishing a strict "no-manual-input" policy: there shall be no manual database schema adjustments. This deterministic approach prevents "silent" data corruption in multi-party settlement systems, where discrepancies in data precision or character encoding between participants could lead to irreversible financial loss. Encoding-level validation at the Gateway acts as our primary defense, ensuring that only messages strictly conforming to the physical description in the Repository are committed to the ledger.

## 2. Mandatory Message Instance & Transport Integrity

To maintain document-level integrity, the following constraints from **ISO 20022-4 (Sections 5.5.1–5.5.3)** are mandatory for all components.

### Message Instance Integrity Requirements

|   |   |   |
|---|---|---|
|Requirement|Implementation Detail|Ledger Component Impact|
|**Encoding**|Must be **UTF-8**|**Gateway/Audit Vault**: Absolute requirement to prevent character-set drift in multi-tenant shards.|
|**XML Prolog**|`<?xml version="1.0" encoding="UTF-8"?>`|**Gateway**: Mandatory identification for parser initialization and security filtering.|
|**DOCTYPE**|**Prohibited**|**Security**: Hard-coded rejection of DOCTYPE to mitigate XML External Entity (XXE) risks.|
|**Namespace Alignment**|`targetNamespace` and `xmlns` (default) must be identical.|**Validation Shards**: URN must follow `urn:iso:std:iso:20022:tech:xsd:[ID]` per **Section 5.3**.|

### Security & Consistency Rationale

The enforcement of UTF-8 ensures that financial metadata—such as complex legal names or multi-currency symbols—remains immutable from the point of ingress to long-term storage in the **Audit Vault**. The prohibition of **DOCTYPE** declarations is a critical security control; by eliminating this feature at the parser level, we neutralize XXE vulnerabilities that could otherwise expose the ledger’s underlying file system or cross-shard network metadata. Furthermore, the requirement for identical `targetNamespace` and default `xmlns` prevents resolution ambiguities during schema-based validation.

## 3. Numeric Precision and "Amount" Storage Requirements

Financial data integrity relies on the precise storage of decimals to prevent truncation during cross-shard settlement. Per **ISO 20022-4 Section 5.7.3.3.2**, "Amounts" must follow a specific structural mapping to `xs:decimal`.

### Amount Storage Constraints (ERD Baseline)

The physical storage layer must implement the `Amount` DataType using a composite structure when a `CurrencyIdentifierSet` is present (the most common B2B case).

- **Simple Type Mapping**: The base value is defined as `[AmountName]_SimpleType`, which is an `xs:restriction` of `xs:decimal`.
- **Mandatory Facets**:
    - `totalDigits`: **18**
    - `fractionDigits`: **5**
- **Composite Implementation**: Per **Section 5.7.3.3.2.1**, when the currency is required, the ERD must implement a composite type (or joined column set) consisting of:
    - The `xs:decimal` value (18, 5 precision).
    - A mandatory attribute **Ccy** (Currency Code), which is a required string.

### Architectural Decision Record (ADR): Amount Handling

**Control Directive**: The **Balance Projection** read model and the **Audit Vault** must never use floating-point types.

1. **Requirement**: Implement all amounts as `DECIMAL(18,5)` paired with a `CHAR(3)` currency identifier.
2. **Constraint**: If the logical model specifies an `ActiveCurrencyAndAmount`, the `Ccy` column must be constrained as `NOT NULL`.

Enforcing these specific digit counts at the database layer prevents the accumulation of rounding errors that would otherwise invalidate settlement finality.

## 4. Temporal Fidelity: Dates and Timestamps

Temporal data must be stored with sufficient fidelity to ensure deterministic ordering within the **Ordering Service / Sequencer**. **ISO 20022-4 (Section 5.7.3.3.9)** and associated examples define the standards for high-fidelity timestamps.

### Fidelity Mapping for Ledger Components

|   |   |   |
|---|---|---|
|ISO 20022 Type|Physical Implementation|Rationale|
|`**xs:dateTime**`|`TIMESTAMP WITH TIME ZONE` (e.g., `2000-12-20T20:00:00Z`)|**Audit Vault/Sequencer**: Mandatory for deterministic ordering. Must include 'Z' (UTC) suffix.|
|`**xs:date**`|`DATE` (YYYY-MM-DD)|**Settlement Logic**: Used for business dates; insufficient for event sequencing.|
|`**xs:gDay**` **/** `**xs:gMonth**`|`INTEGER` or `SMALLINT`|**Scheduling**: Partial components for recurring cycles.|

### Temporal Integrity Rationale

The **Audit Vault** must reject any timestamp lacking an explicit UTC offset (the 'Z' suffix). Without this, the **Sequencer** cannot deterministically break ties between transactions occurring across distributed shards. Using lower-fidelity types like `xs:date` for audit-critical events is strictly forbidden, as it lacks the granularity to prove the order of execution, which is the cornerstone of settlement finality.

## 5. String Constraints and Identifier Integrity

Metadata and identifiers (e.g., BIC, CurrencyCodes) must be protected against buffer overflows and logic bypasses. **ISO 20022-4 (Sections 5.7.3.3.4–5.7.3.3.8)** mandates the use of constraining facets.

### Physical Requirements for the ERD

- **Length Constraints**: `maxLength` and `minLength` must be translated into physical database column limits. For example, `Max35Text` must be implemented as `VARCHAR(35)`.
- **Pattern Validation**: Regex patterns (e.g., `[A-Z]{3,3}` for `CurrencyCode`) must be implemented as database **CHECK** constraints at the storage layer to ensure data sanitization.
- **CodeSets**: `xs:enumeration` values must drive application-level state machines and be reflected as `ENUM` types or check constraints in the database.
- **Relational Integrity (**`**xs:IDREF**`**)**: Per **Section 5.7.2.9**, `xs:IDREF` is used for non-composite relationships. **Architectural Note**: In a sharded ledger, `IDREF` integrity is only guaranteed within the scope of the local XML document or shard. Cross-shard references must be handled via the ledger's global addressing scheme.

## 6. ASN.1 Binary Encoding and Transport Efficiency

While the **Audit Vault** maintains the "Ground Truth" in XML for long-term auditability and readability, the **Inter-Shard Transport** utilizes **ASN.1 (ISO 20022-8)** for high-throughput performance.

### Mandatory Ledger Requirements for Ordering Service

1. **Encoding Protocols**: Default encodings are **Aligned PER** (Packed Encoding Rules) for maximum compression and **XER** (XML Encoding Rules) for syntactical equivalence with XSD.
2. **Structural Mapping**:
    - **MessageComponent** maps to an ASN.1 `SEQUENCE` (**Section 5.8.2.3**).
    - **ChoiceComponent** maps to a `SEQUENCE of CHOICE` (**Section 5.8.2.4**).
3. **Registration and ECN**: Per **Section 5.6.1**, any custom or proprietary encoding must be precisely described using **Encoding Control Notation (ECN)** and registered with the **Registration Authority (RA)**.

The use of Aligned PER for the **Ordering Service** ensures that transaction throughput is maximized while maintaining absolute syntactical compatibility with the XML-based Audit Vault.

## 7. Physical Data Type Requirement Summary (ERD Baseline)

This table serves as the final specification for the Senior Database Engineer.

|   |   |   |   |
|---|---|---|---|
|ISO 20022 DataType|Physical Storage Implementation|Mandatory Facets/Constraints|Component Impact|
|**Amount**|`DECIMAL(18,5)` + `CHAR(3)`|`totalDigits: 18`, `fractionDigits: 5`. `Ccy` NOT NULL.|**Audit Vault**: Precise tracking.|
|**ISODateTime**|`TIMESTAMP WITH TIME ZONE`|ISO 8601 Compliance; Mandatory 'Z' suffix.|**Sequencer**: Tie-breaking.|
|**MaxText (e.g., 35)**|`VARCHAR(35)`|`maxLength` enforced via schema.|**Gateway**: Buffer overflow prevention.|
|**BinaryContent**|`BLOB`|`xs:base64Binary` encoding.|**Audit Vault**: Unstructured data.|
|**IdentifierSet**|`VARCHAR(X)` + `CHECK`|Regex patterns (e.g., BIC patterns) as constraints.|**Validation Shards**: Data sanity.|

### Architecture Decision Record (ADR) Summary

- **MANDATORY**: The **ISO 20022 Repository** is the SSOT; all schemas must be derived via deterministic transformation.
- **MANDATORY**: UTF-8 encoding and the prohibition of DOCTYPE declarations at all ingress points.
- **MANDATORY**: UTC ('Z') offset for all audit-critical timestamps.
- **MANDATORY**: Composite storage for Amounts (Decimal + Currency Code) with 18, 5 precision.
- **MANDATORY**: ASN.1 Aligned PER for high-speed inter-shard transport; XML for the Audit Vault.
- **OUT OF SCOPE**: Manual schema adjustments or the use of non-standard encodings not registered via ECN.