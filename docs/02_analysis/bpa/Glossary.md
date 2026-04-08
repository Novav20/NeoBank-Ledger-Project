# Glossary: Mechatronics to FinTech Bridge

Ordered from foundational to dependent. Each definition only uses terms already defined above, or plain-language wording.

## 1. Business and ecosystem terms
- **B2B**: Business-to-business activity where one company serves another company rather than an individual consumer.
- **Central Authority**: An institution that sets or enforces the rules for a payment or banking ecosystem.
- **Regulators & Auditors**: External parties that inspect evidence, enforce rules, and verify that the system can be trusted.
- **FDIC**: The US agency that insures deposits and supervises bank safety.
- **EBA**: The European Banking Authority, which issues banking supervision guidance in the EU.
- **Sponsor Bank**: The regulated bank that provides the charter, deposit custody, and access to payment rails.
- **Banking-as-a-Service (BaaS)**: A model where a regulated bank exposes banking functions through APIs so another business can build customer-facing products on top.
- **Neobank**: A digital-only bank that delivers banking services through software and APIs instead of branches. In this project, it usually operates through a Sponsor Bank and a BaaS integration layer.
- **Fintech / TPP (Third-Party Provider)**: A software company that integrates with banking rails to provide a financial service.
- **PISP (Payment Initiation Service Provider)**: A TPP that starts a payment on behalf of a customer.
- **SME**: A small or medium-sized enterprise.
- **Corporate Consumer**: A business customer that uses the ledger for operating payments, payroll, or treasury work.
- **KYC**: Know Your Customer, the identity-verification step used at onboarding.
- **AML**: Anti-Money Laundering controls that detect and prevent suspicious financial activity.
- **Nth-Party Risk**: Risk introduced by an indirect partner or service provider rather than by the direct counterparty.

## 2. Ledger and workflow terms
- **Ledger**: The authoritative record of financial events in the system of record.
- **Account**: A named record inside the ledger for one customer or business.
- **Account ID**: The unique identifier for an account.
- **Balance**: The current amount associated with an account.
- **Balance Verification**: The check that account values satisfy the ledger rules.
- **Double-Entry Bookkeeping**: A rule where every financial event creates equal and opposite line items so the ledger stays balanced.
- **Journal Entry**: One balanced record of a financial event in the ledger.
- **Settlement**: The point at which a transfer is treated as final in operations.
- **RTGS (Real-Time Gross Settlement)**: A settlement model where each transfer is settled individually and immediately instead of being netted with others.
- **Straight-Through Processing (STP)**: End-to-end automation with no manual re-entry or handoff.
- **Intent / Command**: A request to change ledger state.
- **Validation**: The rule checks performed before a change is accepted.
- **Journaling**: Writing a validated event into the ledger log.
- **Immutability**: Once written, a record is not edited in place.
- **Cryptographic Provenance**: Verifiable evidence that a record came from a known source and has not been altered.
- **Append-Only Log**: A storage model where new entries are added but existing ones are not overwritten.
- **ACID Properties**: The four guarantees of a database transaction.
    - **Atomicity**: All-or-nothing execution.
    - **Consistency**: Each transaction moves the system from one valid state to another.
    - **Isolation**: Concurrent transactions do not interfere with each other.
    - **Durability**: A committed transaction survives failure.
- **Event Sourcing**: A model that stores each state change as an event and rebuilds current state from event history.
- **CQRS (Command Query Responsibility Segregation)**: A model that uses one path for writes and a separate path for reads.
- **State Projection (Read Model)**: A query-friendly view materialized from the event log.
- **Execute-Order-Validate (EOV)**: A pattern where candidate execution happens first, ordering happens next, validation happens after ordering, and commit happens last.
- **Consensus**: The process by which multiple nodes agree on the same order or state.
- **Sequencer / Orderer**: The service that assigns deterministic order to transactions.
- **DAG (Directed Acyclic Graph)**: A graph of dependencies with no cycles, so work can branch while still preserving order constraints.
- **MVCC (Multi-Version Concurrency Control)**: A concurrency method that keeps multiple versions of data so readers and validators can work against a stable snapshot.
- **Shard**: A partition of ledger state or workload.
- **Validation Shard**: A shard dedicated to conflict checks and consistency verification.
- **Cross-Shard Atomicity**: The guarantee that a transaction touching multiple shards is applied everywhere or nowhere.
- **Epoch Checkpointing**: A periodic snapshot of state used for recovery and audit.
- **Sharded Execution Model**: An architecture that splits work across shards to increase throughput.

## 3. Finality, consensus, and ordering terms
- **Finality Model**: The rule that decides when a transaction can no longer be rolled back.
- **Probabilistic Finality**: Finality that becomes more certain as more confirmations accumulate.
- **Crash Fault Tolerance (CFT)**: The ability to keep working when nodes stop or fail silently.
- **Deterministic Finality**: Finality that is reached immediately once the commit rule is satisfied.
- **Byzantine Fault Tolerance (BFT)**: The ability to keep working when some nodes lie or behave inconsistently.
- **PBFT (Practical Byzantine Fault Tolerance)**: A practical BFT protocol that uses voting rounds to reach commit decisions in permissioned systems.
- **Quorum Certificate (QC)**: A proof signed by enough validators to show the finality rule was met.
- **Committed Finality**: A stage where the transaction is durably stored and can be relied on operationally.
- **Audited Finality**: A stage where the transaction has independent validator evidence suitable for audit and legal review.
- **MEV (Maximal Extractable Value)**: Profit gained by changing transaction order.
- **Orderer Collusion**: When orderer nodes cooperate to bias the order for gain.
- **Fair-Ordering Policy**: A rule that reduces order manipulation and limits MEV.

## 4. Compliance and privacy terms
- **ISO 20022**: An international standard for machine-readable financial messages.
- **LEI (Legal Entity Identifier)**: A global identifier that names the legal entity involved in a transaction.
- **UTI (Unique Transaction Identifier)**: A unique identifier for a specific transaction.
- **Timestamp precision**: The level of exactness used when recording time.
- **MiFID II**: EU market rules that require audit-grade identifiers and timestamp precision for trade events.
- **GDPR**: The EU privacy law that governs how personal data is collected, stored, used, and deleted.
- **Right to Erasure**: The GDPR right that allows personal data to be deleted when the law permits it.
- **Redactable Ledger**: A ledger that can remove or mask personal data while preserving the financial record.
- **PQC (Post-Quantum Cryptography)**: Cryptographic algorithms designed to remain secure against attacks from future quantum computers.
- **Signed Hash-Chain Checkpoint**: A signed checkpoint that links state snapshots together so auditors can verify continuity.

## 5. Storage and architecture terms
- **Merkle DAG**: A directed acyclic graph whose nodes are content-hashed so the structure can be verified.
- **Root Chain**: The main chain that anchors subordinate data structures such as Merkle DAG branches.
- **Private Data Collection (PDC)**: A restricted subset of data shared only with authorized parties.
- **LevelDB**: An embedded key-value store optimized for fast writes.
- **CouchDB**: A document database with flexible querying.
- **Multi-Path Intrusion-Tolerant Overlay**: A redundant network layer that can route around attack or failure.

## 6. Risk and infrastructure terms
- **BGP Hijacking**: Rerouting internet traffic by falsifying route announcements.
- **AS-Level Network Partition**: A split caused by upstream network or routing failures.
- **Floating Point Error**: Inaccuracy caused by using binary floating-point types for money.
- **Network-Bound Ceiling**: A throughput limit caused by network capacity or propagation, not CPU.
- **Bullwhip Effect**: The way small delays or errors become larger as they move through a chain of systems.
