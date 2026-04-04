# Research & Source Harvesting Guide

## 1. The Core Methodology (AI-Augmented Batching)
We follow a 3-batch iterative pipeline to ensure academic rigor and technical depth:
1.  **Ingestion**: Harvest 15-25 high-signal sources per batch.
2.  **Extraction**: Use specialized NotebookLM prompts (in `artifacts/`) for factual data extraction.
3.  **Synthesis**: Generate a **Meta-Analysis** to bridge raw data with architectural artifacts.
4.  **Application**: Update the **BPA Report** and **Requirements** with cited findings.

## 2. Strategic Database Prioritization
Based on UTB availability, these are the prioritized hubs for Ledger Architecture:

1.  **ScienceDirect**: Primary source for peer-reviewed papers on Software Architecture and Database Integrity.
2.  **Emerald Insight**: Best for Business Case Studies on BaaS and Fintech workflows.
3.  **Scopus**: The "Search Hub" to identify highly-cited research (2023-2025).
4.  **JSTOR**: Fundamental Accounting and Economic Theory.
5.  **vLex Colombia**: Regulatory context and Fintech legislation.

## 3. High-Signal Filtering (Avoiding 'System Noise')
To maintain a high "Signal-to-Noise" ratio, apply these filters strictly:

### **A. Journal & Publication Selection**
Prioritize these engineering-focused journals:
-   *Journal of Systems Architecture*
-   *Information Systems*
-   *Computers in Industry*
-   *Journal of Industrial Information Integration*

### **B. Domain Filtering (The "Plumbing" Focus)**
-   **KEEP**: "Event Sourcing", "ACID Compliance", "Distributed Consistency", "Append-Only", "DAGs", "Double-Entry Design".
-   **DISCARD (Noise)**: "AI Chatbots", "Marketing", "Consumer Psychology", "Smart Cities/UAVs" (unless specifically about Ledger storage), "UI/UX".

### **C. Content Type Priority**
1.  **Research Article / Review Article**: Most rigorous.
2.  **Book Chapter**: Excellent for foundational theory.
3.  **Discard**: Conference Abstracts and Reference entries (Dictionary definitions).

## 4. Proven Search Queries
-   `("ledger architecture" OR "immutable database") AND ("transactional integrity" OR "ACID")`
-   `"double entry" AND "consistency" AND "distributed ledger"`

## 5. Batch 02 Focus (Implementation Patterns)
Shift from foundational logic to concrete implementation. Prioritize:
1. **Comparative Architecture Reviews**: DAGs vs Chains, latency and throughput benchmarks. Search: `("DAG" OR "Directed Acyclic Graph") AND "Transaction Throughput"`.
2. **Event Sourcing & CQRS**: State machine replication for financial audit trails. Search: `"Event Sourcing" AND "Financial Accounting" AND "Consistency"`.
3. **Concurrency Control & ACID**: MVCC/OCC for distributed databases during high-concurrency ledger updates. Search: `"ACID compliance" AND "concurrency control" AND "financial ledger"`.
4. **Integrity Verification & Self-Repair**: Vector commitments and cryptographic proofs of integrity. Search: `"Cryptographic Proof" AND "Audit Log Integrity"`.

## 6. Batch 03 Focus (NFRs & Compliance)
Focus on operational assurance, regulatory safety, and performance ceilings. Prioritize:
1. **Availability & Fault Tolerance**: Shard recovery, leaderless sync, and partition resilience. Search: `("availability" OR "fault tolerance") AND "distributed ledger" AND "shard takeover"`.
2. **Performance Benchmarks**: Latency (ms) and throughput (TPS) metrics for B2B financial systems. Search: `("latency" OR "throughput") AND "B2B" AND "distributed ledger" AND "benchmark"`.
3. **Compliance by Design**: ISO 20022 alignment, machine-readable rules, and automated auditing. Search: `("ISO 20022" OR "regulatory compliance") AND "ledger architecture" AND "automation"`.
4. **Safety & Double-Spending**: State coherence and transactional safety in distributed environments. Search: `("double spending" OR "state coherence") AND "distributed ledger" AND "safety proofs"`.
