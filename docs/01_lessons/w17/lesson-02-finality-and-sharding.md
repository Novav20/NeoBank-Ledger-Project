# Lesson 02: Deterministic Finality and Distributed Sharding

## Introduction
In a high-integrity ledger, we face two engineering challenges:
1. **Agreement**: How do we know a transaction is truly "Done" and cannot be reversed? (**Finality**)
2. **Scale**: How do we process 10,000 transactions per second without a single-point-of-failure bottleneck? (**Sharding**)

---

## 1. Finality: The "Mechanical Limit Switch"

### Probabilistic Finality (The "Software Sensor")
In systems like Bitcoin, finality is probabilistic. It's like a noisy sensor; the longer you wait, the more likely the reading is correct, but there is always a chance of a "re-org" (reversal). This is unacceptable for B2B banking.

### Deterministic Finality (The "Hard Stop")
We use **PBFT (Practical Byzantine Fault Tolerance)**. 
- **The Physics**: It requires a **Quorum Certificate (QC)**.
- **The Rule**: In a cluster of $N$ nodes, we need $2f+1$ signatures to commit (where $f$ is the number of faulty nodes we can tolerate).
- **Mechatronics Analogy**: Think of a safety-critical hydraulic press. It won't fire until three separate physical switches are pressed simultaneously. Once it fires, the action is physical and irreversible.

---

## 2. Distributed Consensus: PBFT vs. HotStuff

As an engineer, you care about **Complexity**:
- **PBFT ($O(n^2)$)**: Every node talks to every other node. It's great for small, high-precision clusters (like our 4-7 node MVP).
- **HotStuff ($O(n)$)**: Nodes talk to a leader, who aggregates signatures. This is like a "Master-Slave" PLC architecture. We will switch to this if our network grows beyond 32 nodes (**Chan et al., 2018**).

---

## 3. Sharding: The "Multi-Cylinder Engine"

If one shard (one CPU/Node) is the bottleneck, we split the work. But how do we handle a transaction between Shard A and Shard B?

### The Traditional Way: 2PC (Two-Phase Commit)
This is slow. It’s like a mechanical linkage where every joint adds friction and delay.

### The MSSP Way: m-nodes (**Liu et al., 2025**)
We use **m-nodes** (Multi-shard nodes). 
- **The Concept**: Some nodes belong to **both** shards. 
- **The Optimization**: Research shows that if **20%-25%** of your nodes are m-nodes, they can "bridge" the shards instantly without a slow 2PC handshake.
- **Mechatronics Analogy**: Think of a gearbox. Instead of having two separate independent shafts, we have a "Common Gear" that touches both systems, allowing power to flow between them without a separate clutch mechanism.

---

## Technical Summary for ADR-002/003
| Concept | Technical Choice | Engineering Reason |
| :--- | :--- | :--- |
| **Finality** | PBFT (Deterministic) | Eliminates the risk of "Transaction Reversal." |
| **Fault Tolerance** | Byzantine (BFT) | Protects against "Malicious Data" (Equivocation). |
| **Scaling** | Sharding (MSSP) | Parallelizes throughput via m-nodes. |
| **m-node Ratio** | 20% - 25% | Peak efficiency for cross-shard atomicity. |
