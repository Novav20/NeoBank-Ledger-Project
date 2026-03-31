---
source:
  - Al-Bassam (2018) | Sonnino (2021) | Barger (2021) | Guggenberger (2022)
phase: performance
status: draft
last-updated: 2026-03-31
applied-in-project: yes
---

# Lesson 04: Performance Anchors (Benchmarks & Limits)

## Objective
Understand the technical boundaries of high-performance ledgers. We define "Fast" using research-backed numbers (TPS and Latency) to set realistic engineering goals.

## Why It Matters for the Ledger
- **Scale**: A B2B ledger might handle thousands of transactions per second during payroll runs.
- **Expectation Management**: We must know the difference between "LAN speed" (fast) and "WAN speed" (slow).
- **Bottlenecks**: Knowing where the system will break (CPU vs. Network) allows us to design better sharding strategies.

## Key Concepts

### 1. TPS (Transactions Per Second)
The "Speedometer" of the ledger.
- **Our Baseline**: **2,500 TPS** (Standard BFT consensus in a local network).
- **The Apex**: **160,000+ TPS** (Specialized protocols like FastPay).
- **The Reality**: Most B2B needs are met by 1,000–5,000 TPS, provided latency is low.

### 2. Latency (The Delay)
How long it takes for a transaction to be "Final."
- **Intra-Cluster (LAN)**: **< 100ms - 200ms**. This is our target for high-performance modules.
- **Cross-Region (WAN)**: **1.2s - 1.5s**. This is the physical limit imposed by the speed of light and network hops across continents.

### 3. The Consensus Penalty
Every time you add a "Validator" node to make the system safer, it gets **slower**.
- **The Limit**: At **$n \geq 32$ nodes**, the network becomes "Bandwidth Bound." The time spent talking between nodes exceeds the time spent processing data.

## Mental Model: The Highway Analogy
- **TPS**: The number of cars passing a point per second (**Throughput**).
- **Latency**: How long it takes for *one* car to get from the entrance to the exit (**Travel Time**).
- **The Bottleneck**: A 10-lane highway (High TPS) doesn't matter if the toll booth (Consensus) only processes 1 car per minute (High Latency).

## Performance Benchmarks (The "Cheat Sheet")
| Protocol Style | TPS Target | Latency Target | Source |
| :--- | :--- | :--- | :--- |
| **FastPay (RTGS)** | 160,000 | < 100ms | Sonnino (2021) |
| **BFT-SMART (Fabric)** | 2,500 | ~133ms | Barger (2021) |
| **Raft + LevelDB** | > 1,000 | 1.2s - 1.5s (WAN) | Guggenberger (2022) |

## Common Pitfalls
- **Over-Engineering**: Trying to hit 100,000 TPS when the business only does 10 transactions per minute.
- **Ignoring WAN Latency**: Designing a system that works in the lab but fails when nodes are in different countries.
- **Database Bottlenecks**: Using a slow storage engine (like CouchDB) can reduce performance by 3x compared to LevelDB.

## Interview Notes
- **Throughput vs. Latency**: Throughput is total work per unit of time; Latency is time for a single unit of work.
- **Finality**: The moment a transaction cannot be reversed.
- **Scalability**: The ability to increase TPS by adding more hardware or shards.

## Sources
- [[al_bassam_2018|Al-Bassam, 2018]]: On intra-cluster latency targets.
- [[sonnino_2021|Sonnino, 2021]]: High-speed RTGS benchmarks (FastPay).
- [[barger_2021|Barger, 2021]]: Production baselines for BFT systems.
- [[guggenberger_2022|Guggenberger, 2022]]: WAN performance and storage engine impacts.

## TODO to Internalize
- [ ] Calculate how many transactions per day a system does if it runs at 1,000 TPS.
- [ ] Explain why a 7-node cluster is often the "Sweet Spot" for BFT performance.
- [ ] Research why LevelDB is faster than traditional SQL for append-only logs.
