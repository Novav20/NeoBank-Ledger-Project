# Production Blueprint: High-Integrity B2B Ledger Domain Model

### 1. Strategic Alignment: ISO 20022 Metamodel to Ledger Architecture

The architectural integrity of a high-integrity B2B ledger necessitates a paradigm shift from "message-centric" transport to a "model-centric" domain architecture. By adopting the ISO 20022-2 UML profile, we establish a formal semantic foundation where the ledger’s runtime behavior is a direct execution of technology-independent business logic. This transition ensures deterministic state updates within the Sequencer: state transitions are governed by the conceptual state of _BusinessComponents_ (Level 2) rather than the physical syntax (XML/ASN.1) of the transport layer. This decoupling allows the ledger to maintain a "Single Version of the Truth" that remains immutable regardless of the underlying messaging technology.

The mapping of ISO 20022 Metamodel levels to our production runtime is defined as follows:

|   |   |   |
|---|---|---|
|ISO 20022 Level / Metaclass|Ledger System Component|Architectural Logic|
|**Message Level (Logical)**|**Gateway / Ingress**|Intercepts external syntax; maps `MessageElements` to internal logical structures for schema validation.|
|**Conceptual Dynamic**|**Ordering Service / Sequencer**|Resolves race conditions by translating `BusinessTransaction` flows and `MessageTransportMode` into a deterministic execution sequence.|
|**Metamodel::Constraint**|**Validation Shards**|Executes OCL (Object Constraint Language) logic. Constraints are stored as `OpaqueExpressions` within the Audit Vault and parsed at runtime to verify state transition legality.|
|**Conceptual Static**|**Balance Projection (State)**|The materialized read-model representing the current valid state of all `BusinessComponents` and `BusinessElements`.|
|**Metamodel::Repository**|**Audit Vault**|The immutable "Source of Truth" for `RepositoryConcept` metadata, OCL expressions, and the historical evidence trail.|

### 2. Domain Entity Rules: BusinessComponents and Aggregates

In this architecture, `BusinessComponents` are the definitive source for the ledger’s Entity-Relationship Diagram (ERD). They provide exhaustive definitions of financial notions, ensuring that data structures remain stable even as specific message contexts evolve.

#### Architecture Decision Record (ADR) 001: Aggregation Logic for Financial Balances

- **Status**: Mandatory
- **Context**: ISO 20022-3 (Section 6.2.1) requires clear definition of business containment.
- **Decision**: The relationship between "Account" and "Balance" must utilize **COMPOSITE** aggregation.
- **Consequences**: A Balance cannot exist independently of an Account; the lifecycle of the Balance is strictly bound to the Account entity. This prevents orphaned state entries and ensures referential integrity during shard pruning.

#### Architecture Decision Record (ADR) 002: Association Navigability and Graph Traversal

- **Status**: Mandatory
- **Context**: Efficient cross-shard atomicity requires predictable entity traversal paths.
- **Decision**: Per ISO 20022-2 Section A.2.5.3, every `BusinessElement`-stereotyped association end **shall be navigable**, and the opposite end **must also** be stereotyped as a `BusinessElement`.
- **Consequences**: This ensures the ledger’s validation engine can perform bidirectional graph traversal between components (e.g., from Transaction to Party and vice versa) without non-deterministic lookups.

#### Architecture Decision Record (ADR) 003: Permissible Aggregation Types

- **Status**: Mandatory
- **Context**: Business-level associations must reflect real-world independence.
- **Decision**: `BusinessAssociation` types are restricted to **NONE** or **COMPOSITE**. The **SHARED** aggregation type is strictly forbidden at the Business Component level (Section A.2.5.3).
- **Consequences**: Components like "Party" and "Account" must use `NONE` aggregation to reflect their independence. `SHARED` aggregation is reserved exclusively for the `TypeLibrary`.

### 3. Data Precision and Primitive Constraints

Financial ledgers require absolute precision to eliminate rounding discrepancies and transport corruption. All ledger ingress must be validated against the 14 built-in ISO 20022-2 Section 5.10 DataTypes: `base64Binary`, `boolean`, `date`, `dateTime`, `decimal`, `duration`, `gDay`, `gMonth`, `gMonthDay`, `gYear`, `gYearMonth`, `integer`, `string`, and `time`.

**Mandatory Ledger Data Types and NFRs:**

|   |   |   |
|---|---|---|
|Type|Mandatory Attributes (ISO 20022-2)|Ledger Implementation / NFR|
|**Amount**|`currencyIdentificationSet`|Enforce precise decimal scaling; mandatory currency ISO code validation.|
|**Indicator**|`meaningWhenTrue`, `meaningWhenFalse`|Strict boolean logic; requires explicit semantic mapping for auditability.|
|**Quantity**|`unitCode`|Non-negative integer validation; mandatory unit-of-measure checks.|
|**Rate**|`baseValue`, `baseUnitCode`|High-precision decimal (min 12 decimal places) for interest/exchange rate validation.|
|**Text**|_(No Properties)_|Section 5.9.6: Constraints must be applied via Restriction Stereotypes (e.g., `length`, `pattern`).|

**Validation Constraints (Gateway Ingress):** The Gateway must enforce "Restriction Stereotype" logic (Section 5.10) for all `Binary`, `Date`, and `Decimal` types. Every transaction must satisfy XML Schema Constraining Facets—specifically `totalDigits`, `fractionDigits`, and `pattern`—as mandatory acceptance criteria.

**CodeSet vs. IdentifierSet Resolution:** While `IdentifierSets` allow for simple string matching (low semantic context), `CodeSets` are treated as immutable domain entities. The Audit Vault must resolve the `ownedLiteral` (Section 5.2.3) for every `CodeSet` to ensure the business meaning of a value is preserved across the ledger's lifecycle.

### 4. Deterministic Ordering and Transport Integrity

To ensure settlement finality, the Ordering Service translates `MessageTransportMode` (Section 5.5.4) into functional requirements for the Sequencer and Validation Shards.

- **MessageDeliveryOrder**: Mandatory requirement for the Sequencer. It must resolve all race conditions to produce a single, deterministic transaction stream.
- **DeliveryAssurance & Durability**: Core NFRs for the Audit Vault; guaranteed persistence is required before any state update is acknowledged.
- **MessageValidationLevel & MessageValidationOnOff**: Mandatory NFRs for Validation Shards. These parameters define the depth of business rule checking (e.g., `STRICT` vs `LAX`) performed during local consensus.
- **BoundedCommunicationDelay**: Recommended control for cross-shard synchronization to bound the temporal window for cross-entity validation.

### 5. Compliance, Traceability, and the Audit Vault

The Audit Vault serves as the immutable repository for all ledger activity, grounded in the `RepositoryConcept` (Section 5.2.9).

**Mandatory Metadata for Ledger Entries:**

1. **objectIdentifier**: A unique, immutable string for every concept.
2. **registrationStatus**: Must track the lifecycle of every entry (`PROVISIONALLY_REGISTERED`, `REGISTERED`, or `OBSOLETE`).
3. **removalDate**: If a `removalDate` is present, the entry status **must** be updated to `OBSOLETE` (Section A.2.2.7), supporting regulatory data lifecycle requirements.
4. **semanticMarkup**: Mandatory for all entries to provide context for regulatory oversight.

**Constraint Logic:** All `Constraint` mappings (Section 5.2.5) must be stored in the Audit Vault as `OpaqueExpressions` (Section A.2.2.4). These OCL expressions define the logic by which Validation Shards reject non-compliant commands.

**Strategic Traceability and Cardinality:** Per Section A.2.6.2, every `MessageElement` (Logical) must be traced to a `BusinessElement` (Conceptual). The **Cardinality Alignment** rule is a hard-coded validation constraint: a trace may **only** raise the minimum cardinality (making an optional field mandatory) or lower the maximum cardinality (restricting a list). Any transaction attempting to violate these cardinality bounds must be rejected by the Validation Shards.

This blueprint constitutes the final "Source of Truth" for the production ledger, ensuring every domain entity remains structurally and semantically grounded in ISO 20022 standards.