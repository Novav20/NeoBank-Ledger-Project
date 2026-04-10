---
version: 1.2
last_modified: 2026-03-29
---
	
# Business Process Analysis (BPA) Report: Neobank Ledger API

## 1. Introduction and Context

### Project Objectives

The objective of this project is to instantiate a high-integrity, B2B-grade ledger system that serves as a "common semantic layer" for Banking-as-a-Service (BaaS) architectures.

- **Integrity**: Ensure 100% transaction accuracy using double-entry bookkeeping.
- **Auditability**: Provide evidence-grade logging to satisfy regulatory mandates (e.g., FDIC custodial account requirements).
- **Automation**: Enable Straight-Through Processing (STP) to eliminate manual reconciliation gaps.

### Scope

The system covers the internal ledgering of funds within a Neobank ecosystem, including Account management, Journal Entry instantiation, and Balance verification.

- **In-Scope**: Digital ledgering, double-entry validation, balance management, and metadata extensibility.
- **Out-of-Scope**: Physical payment rail execution (Fedwire/RTGS), Customer KYC/Onboarding, and front-end UI.

### Stakeholders & Ecosystem

The ecosystem is evolving from solitary institutional silos to federated platform networks. The primary relationships map as follows:

- **Sponsor Banks & Central Authorities**: Serve as the foundational trust anchors and regulatory interfaces. They are evolving into platforms (Banking-as-a-Platform) for external integration and visibility into the "system of record" ([[alfhaili_2025|Alfhaili et al., 2025]]; [[huang_2024|Huang et al., 2024]]; [[renduchintala_2022|Renduchintala et al., 2022]]).
- **Fintech Corporations & Third-Party Providers (TPPs)**: Agile platforms acting as innovators and intermediaries, executing specialized roles such as Payment Initiation Service Providers (PISPs). They launch neobanks that require modular infrastructure and frequently rely on sponsor banks for regulatory cover and capital buffering ([[walker_2023|Walker et al., 2023]]; [[bamakan_2020|Bamakan et al., 2020]]).
- **SMEs & Corporate Consumers**: End-users who demand transparent, fast, and unbundled financial services. They suffer most from current "As-Is" settlement delays and high transaction costs under various consensus/sharding trade-offs ([[fatorachian_2025|Fatorachian et al., 2025]]; [[singh_2022|Singh et al., 2022]]; [[nasir_2022|Nasir et al., 2022]]).
- **Regulators & Auditors**: Critical actors enforcing safety and soundness (FDIC, EBA, ISO 20022), transitioning from post-hoc manual verifiers to real-time compliance monitors using machine-readable standards and accountability schemes like reputation-based witness selection and **signed hash-chain checkpoints** for continuous oversight ([[boukhatmi_2025|Boukhatmi & Van Opstal, 2025]]; [[mashiko_2025|Mashiko et al., 2025]]; [[wang_2025|Wang et al., 2025]]; [[mohsenzadeh_2022|Mohsenzadeh et al., 2022]]; [[sonnino_2021|Sonnino, 2021]]; [[noreen_2023|Noreen et al., 2023]]).

## 2. Analysis of the Current State (As-Is)

### Foundational Logic (As-Is)

Despite the shift toward distributed architectures, the automated transaction lifecycle relies on formalizing logic heavily influenced by legacy constraints. Current manual or legacy-based processes involve high degrees of "exception handling" due to unstructured data. The traditional lifecycle operates as follows:

1. **Intent (Transaction Initiation)**: Triggered by user platforms or APIs, often facing high friction due to decentralized identity and KYC/AML verification requirements ([[cisar_2025|Cisar et al., 2025]]).
2. **Validation (Consensus & Verification)**: Requires consensus and adherence to strict access controls to overcome the "trust deficit" between non-affiliated parties ([[quamara_2024|Quamara et al., 2024]]; [[rage_2022|Rage et al., 2022]]).
3. **Journaling (Data Recording)**: Moves from siloed double-entry databases to shared, immutable, append-only logs ensuring system-wide state consistency ([[fulbier_2023|Fülbier & Sellhorn, 2023]]).
4. **Settlement**: The finalization of state, previously taking multiple days (often T+2), now aiming for near real-time execution via smart contracts, though frequently constrained by network latency and cross-ledger interoperability barriers ([[cisar_2025|Cisar et al., 2025]]; [[quamara_2024|Quamara et al., 2024]]).

### "As-Is" Process Map

```mermaid
sequenceDiagram
    participant P as Platform/Fintech
    participant M as Manual/Legacy System
    participant B as Sponsor Bank

    P->>M: Send Unstructured Payment Intent
    M->>M: Manual Data Validation
    M->>M: Entry in Spreadsheet/Legacy DB
    Note over M: Potential for Floating Point Errors
    M->>B: Batch File Upload (EOD)
    B-->>M: Reconciliation Errors / Exceptions
    M-->>P: Delayed Settlement Notification
```

### Data Inventory

Current "As-Is" data often lacks structure, but the transition to ISO 20022 mandates:

- **Account ID**: Unique identifier.
- **Amount**: Often stored as floats (Error source); must transition to Integers.
- **Currency**: ISO 4217 code.
- **Timestamps**: Event creation/update.

### Pain Points (System Entropy)

The researched domain presents several critical risks and inefficiencies:

- **Nth-Party Risk & Accountability Gaps**: As ecosystems expand, accountability gaps emerge where autonomous agents or intermediate service providers may introduce vulnerabilities or incorrect data without immediate traceability. This includes **MEV (Maximal Extractable Value) / Orderer Collusion** where adversarial nodes reorder transactions to extract value ([[boukhatmi_2025|Boukhatmi & Van Opstal, 2025]]; [[li_2024|Li & Pournaras, 2024]]).
- **Infrastructure & Transport Hazards**: Inter-ledger flows are vulnerable to **BGP Hijacking** and AS-level network partitions, which can bypass traditional encryption-only security models and disrupt settlement availability ([[trestioreanu_2021|Trestioreanu et al., 2021]]).
- **Reconciliation Gaps & Latency**: Legacy sequential models induce the "bullwhip effect" in data distribution, causing mismatches across isolated organizational ledgers and requiring extensive manual resolution ([[alt_2025|Alt & Gräser, 2025]]; [[seshadrinathan_2025|Seshadrinathan & Chandra, 2025]]).
- **Data Integrity & Privacy Loss**: The tension between the transparency requirement of ledgers and business confidentiality introduces mapping uncertainty, information overload, and regulatory exposure. This is exacerbated by the **GDPR vs. Immutability** conflict where the right to erasure contradicts append-only audit trails ([[alfhaili_2025|Alfhaili et al., 2025]]; [[fulbier_2023|Fülbier & Sellhorn, 2023]]; [[zhao_2024|Zhao et al., 2024]]).
- **Scalability, Technical Bottlenecks & Floating Point Errors**: Inaccuracy in multi-currency rounding and high transaction confirmation latency. Contract vulnerabilities and linear storage growth limit the throughput in high-frequency B2B contexts where orphan blocks or propagation limits occur. **Network-bound ceilings** typically emerge at $n \geq 32$ nodes, shifting bottlenecks from CPU to bandwidth ([[huang_2024|Huang et al., 2024]]; [[madlberger_2025|Madlberger et al., 2025]]; [[wu_2022|Wu et al., 2022]]; [[fan_2025|Fan et al., 2025]]; [[berger_2023|Berger et al., 2023]]; [[wang_2024|Wang et al., 2024]]).

## 3. Gap Analysis

- **Current (As-Is)**: Measured in quarters for launch; manual exception handling; unstructured messaging.
- **Future (To-Be)**: Measured in weeks for launch; Straight-Through Processing (STP); ISO 20022 structured data.
- **Improvement Opportunity**: Automate the "Accounting Equation" validation (Debits = Credits) at the API level.

## 4. System Requirements & Business Rules

### 4.1 Foundational Principles (Core Business Rules)

To build a robust, high-transaction B2B Ledger, the architecture must strictly enforce the following non-negotiable principles:

- **Double-Entry Balance**: Every transaction must maintain perfect equilibrium between debits and credits; the dual-entry axiom remains the unbreakable core of all financial systems ([[fulbier_2023|Fülbier & Sellhorn, 2023]]; [[mashiko_2025|Mashiko et al., 2025]]).
- **Immutability & Cryptographic Provenance**: An append-only historical record is mandatory to ensure tamper-resistance and facilitate automated auditing; Merkle committing and cryptographically chaining remain non-negotiable anchors. For B2B compliance, the system MUST distinguish between **Committed** (CFT-durable) and **Audited** (BFT-verified) finality ([[alt_2025|Alt & Gräser, 2025]]; [[mishra_2025|Mishra et al., 2025]]).
- **Integer Precision**: To prevent fractional data loss and rounding errors, financial quantities must be managed using integer arithmetic ([[huang_2024|Huang et al., 2024]]).
- **Explicit Finality Model**: The platform must declare if settlement is probabilistic or deterministic. For B2B, **deterministic finality** via Quorum Certificates (QC) is required ([[bouraga_2021|Bouraga, 2021]]; [[berger_2023b|Berger et al., 2023b]]).
- **ISO 20022 and MiFID II Compliance**: All inter-ledger messages MUST adhere to ISO 20022 formats. Trade events MUST carry LEI/UTI identifiers and 1ms-precision timestamps ([[chuen_2017|Lee & Deng, 2017]]).

### 4.2 Consolidated NFR Registry

Derived from Meta-Analysis of 37 technical sources:

| ID            | Category     | Statement                                                 | Primary Source(s)               | Evidence Mode |
| ------------- | ------------ | --------------------------------------------------------- | ------------------------------- | ------------- |
| NFR-AVAIL-01  | Availability | $3F+1$ nodes, BFT, view-change <30s                       | [[barger_2021]]; [[berger_2023b]]; [[chuen_2017]] | Synthesis |
| NFR-PERF-01   | Performance  | <210ms intra-cluster latency (normal load)                | [[al_bassam_2018]]                | Direct |
| NFR-PERF-02   | Performance  | ≥2,500 TPS in 7-node LAN                                  | [[barger_2021]]                   | Direct |
| NFR-PERF-03   | Performance  | τ₀ ≥ 3σ consensus timer                                   | [[chan_2018]]                     | Direct |
| NFR-PERF-04   | Performance  | <100ms target, 1.5s WAN ceiling (tiered SLA)              | [[sonnino_2021]]; [[guggenberger_2022]] | Synthesis |
| NFR-SAFE-01   | Safety       | QC-enforced finality, double-spend impossible             | [[berger_2023b]]; [[benedetti_2022]]; [[praveen_2024]] | Synthesis |
| NFR-SAFE-02   | Safety       | Committed vs. Audited finality formally distinguished     | [[mishra_2025]]                   | Direct |
| NFR-SAFE-03   | Safety       | Formal verification (IVy/Z3) of fork-freedom              | [[praveen_2024]]                  | Direct |
| NFR-AUDIT-01  | Audit        | $2F+1$ QC per block, independently auditable              | [[barger_2021]]; [[praveen_2024]]; [[benedetti_2022]] | Synthesis |
| NFR-COMP-01   | Compliance   | ISO 20022 inter-ledger messaging                          | [[chuen_2017]]; [[sonnino_2021]] | Direct |
| NFR-COMP-02   | Compliance   | LEI + UTI + 1ms timestamp per trade event (MiFID II)      | [[chuen_2017]]                    | Direct |
| NFR-COMP-03   | Compliance   | Fair-ordering policy on Ordering Service (MEV mitigation) | [[li_2024]]                       | Direct |
| NFR-COMP-04   | Compliance   | RTGS settlement model (no net-settlement exposure)        | [[sonnino_2021]]                  | Direct |
| NFR-COMP-05   | Compliance   | Post-quantum cryptographic migration roadmap              | [[wang_2026]]; [[mishra_2025]]    | Synthesis |
| NFR-RISK-01   | Risk         | OSS dependency SLA + CVE patching policy                  | [[chuen_2017]]; [[mishra_2025]]   | Synthesis |
| NFR-CONFIG-01 | Config       | LevelDB over CouchDB; PDC only if justified               | [[guggenberger_2022]]             | Direct |
| NFR-CONFIG-02 | Config       | 20–25% m-node shard ratio                                 | [[liu_2025]]                      | Direct |
| NFR-CONFIG-03 | Config       | PBFT k=7 shards at 1,000 nodes                            | [[wang_2026]]                     | Direct |
| NFR-STORE-01  | Storage      | Epoch checkpointing; per-node cap defined at design time  | [[nasir_2022_batch03]]; [[al_bassam_2018]]; [[sonnino_2021]] | Synthesis |
| NFR-INFRA-01  | Infra        | Multi-path intrusion-tolerant inter-ledger overlay        | [[trestioreanu_2021]]             | Direct |

## 5. Technical Architecture & "To-Be" Model

### 5.1 Architectural Posture

The "To-Be" state combines an **Event Sourcing** ledger core with a **sharded execution model** and a **Fabric-like Execute-Order-Validate (EOV)** pipeline. Commands are accepted as immutable business events, ordered by a sequencer/orderer service (often via root-chain Merkle DAGs), validated for conflicts and policy, and appended to an immutable log that continuously materializes query-ready state views. This prevents dangerous cycles and stale reads through conflict-aware ordering ([[kahmann_2023|Kahmann et al., 2023]]; [[wu_2026|Wu et al., 2026]]; [[liao_2026|Liao et al., 2026]]; [[li_2023a|Yi Li et al., 2023]]; [[zhang_2026|Zhang et al., 2026]]; [[conti_2022|Conti et al., 2022]]).

### 5.2 To-Be Flow (Event Sourcing + Shards + Fabric-like EOV)

1. **Command & Intent Capture**: API command is authenticated, normalized to structured event payload, and checked for schema/compliance gates.
2. **Execute (Pre-Validation)**: Candidate transaction effects are simulated against current versioned state to build read/write sets.
3. **Order (Sequencer)**: Transactions are globally ordered by the sequencer with deterministic metadata for replay and audit.
4. **Validate (Shard / Conflict Control)**: Validation shards apply MVCC-style checks, conflict dependency rules, and cross-shard atomicity controls before commit.
5. **Append & Materialize**: Accepted transactions are appended to the immutable event log and projected to account balances / reporting views for low-latency reads.
6. **Settle & Observe**: Finality status, audit proofs, and exception signals are emitted for operational monitoring and regulator-facing evidence trails ([[bouraga_2021|Bouraga, 2021]]; [[ding_2025|Ding et al., 2025]]; [[wu_2026|Wu et al., 2026]]).

### 5.3 "To-Be" Sequence Diagram

```mermaid
sequenceDiagram
    autonumber
    participant C as Client/Fintech API
    participant G as Ledger API Gateway
    participant E as Event Store (Append-Only Log)
    participant S as Sequencer / Orderer
    participant V1 as Validation Shard A
    participant V2 as Validation Shard B
    participant P as State Projection (Balances/Read Model)

    C->>G: Submit Command (transfer/journal)
    G->>E: Persist Command Event (immutable)
    E-->>S: Event Reference + Metadata
    S->>V1: Ordered Tx Batch (partition A)
    S->>V2: Ordered Tx Batch (partition B)
    V1->>V1: MVCC + Dependency Checks
    V2->>V2: MVCC + Dependency Checks
    V1-->>S: Validation Result + Proof
    V2-->>S: Validation Result + Proof
    S->>E: Commit Ordered Validated Events
    E->>P: Project State Updates
    P-->>C: Finality Status + Updated Balances
```

## 6. Metrics & Value Expected

### 6.1 Performance Benchmarks

Empirical performance anchors derived from research establish the following feasibility targets:

| Context                       | Protocol           | TPS     | Latency                 | Source            |
| ----------------------------- | ------------------ | ------- | ----------------------- | ----------------- |
| **Aspirational / Apex**       | FastPay (RTGS)     | 160,000 | <100ms (LAN/Intra-cont) | sonnino_2021      |
| **TEE-Gated Upper Bound**     | Proteus (PFT)      | 345,000 | 2.6x lower than BFT     | mishra_2025       |
| **Production Baseline (LAN)** | BFT-SMART (Fabric) | 2,500   | ~133ms                  | barger_2021       |
| **Production Baseline (WAN)** | RAFT + LevelDB     | >1,000  | 1.2s – 1.5s             | guggenberger_2022 |
| **Sharded Baseline**          | MSSP (14 shards)   | 2,654   | N/A                     | liu_2025          |

### 6.2 Tiered SLA Structure

The system will offer tiered Service Level Agreements based on the network topology:

- **Intra-Cluster / Single Region**: <100ms latency, ≥2,500 TPS target.
- **Cross-Organization / WAN Settlement**: <1.5s latency, ≥1,000 TPS target.
- **Conflict Handling**: LevelDB-optimized write pipeline to mitigate the 3x penalty of alternative storage engines.

## 7. Gap Analysis - Future Work

### 7.1 Pending Design Conflicts (ADR Candidates)

- **Immutability vs. GDPR**: Resolving the conflict between immutable audit trails and the "Right to Erasure" (Article 17). Exploration of **Redactable Ledgers** (e.g., Concordit) as a pivot ([[zhao_2024|Zhao et al., 2024]]).
- **Committed vs. Audited Finality**: Defining the legal settlement threshold for B2B contracts.
- **Post-Quantum Cryptography (PQC)**: Planning the migration roadmap to hardware-accelerated PQC to protect long-term ledger integrity ([[wang_2026|Wang et al., 2026]]).

### 7.2 Research Continuity

- **Batch 04 Integration**: Continuous monitoring of OSS dependency vulnerabilities and security patching SLAs ([[chuen_2017|Lee & Deng, 2017]]).
