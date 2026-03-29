# Meta-Analysis: Batch 03 (NFRs & Regulatory Compliance)

**Output path:** `docs/00_meta/orchestration/responses/w13/03-batch-03-meta-analysis.md`
**Protocol:** Iterative Sliding-Window (9 sources per block)
**Total sources in batch:** 37

---

## 1. Executive Summary (Initialized - Block 01)

Batch 03 focuses on the **Non-Functional Requirements (NFRs) and Regulatory Compliance** dimensions of the B2B Ledger API, directly complementing Batch 02's structural and implementation patterns. The first nine sources establish a clear, recurring tension between **BFT protocol scalability and the hard latency/throughput SLAs** required for real-world financial infrastructure.

A dominant finding is the **network-bound ceiling of BFT at scale**: as the replica count exceeds 32 nodes, bandwidth becomes the primary bottleneck—not CPU—a constraint that fundamentally shapes the NFR landscape for any high-throughput ledger ([[berger_2023|Berger et al., 2023]]; [[berger_2023a|Berger et al., 2023a]]). **Sharding** emerges as the canonical horizontal scaling solution, but introduces a critical counterforce: the **shard takeover attack surface** ([[berger_2023b|Berger et al., 2023b]]). **Cross-chain standardization** (Alzahrani) and **smart-contract-level compliance** (Bernauer/Daml) provide the compliance and auditability layer above the consensus substrate.

---

## 2. Cross-Source Synthesis

### 2.1 Availability & Fault Tolerance

The sources converge on **Byzantine Fault Tolerance (BFT)** as the non-negotiable baseline for permissioned financial ledgers, with all protocols requiring a minimum of $N = 3F + 1$ nodes to tolerate $F$ Byzantine failures.

- **View-change as the universal liveness mechanism**: All BFT protocols surveyed (PBFT, HotStuff, Kauri, FBFT, BFT-SMART) rely on a view-change sub-protocol to elect a new leader when the primary fails. The quality of this mechanism directly determines downtime duration ([[barger_2021|Barger et al., 2021]]; [[benedetti_2022|Benedetti et al., 2022]]).
- **Leaderless as fault-avoidance**: BECP addresses the leader-failure problem by eliminating leaders entirely, using epidemic communication at the cost of a higher base latency (~10s) and lower throughput (~0.096 blocks/s) ([[abdi_2025|Abdi et al., 2025]]). This trade-off makes leader-based protocols preferable for performance-critical B2B ledgers.
- **Shard-level availability**: Chainspace demonstrates that sharding with BFT per-shard achieves linear throughput scaling, but the **shard takeover** vulnerability identified in the SoK (Berger et al., 2023b) means each shard must be sized conservatively ($3F+1$) ([[al_bassam_2018|Al-Bassam et al., 2018]]; [[berger_2023b|Berger et al., 2023b]]).
- **Cross-chain failover**: Alzahrani's DSM introduces an automated failover mechanism where, if a validation node fails during cross-chain communication, another node automatically assumes responsibility, maintaining availability for inter-ledger B2B flows ([[alzahrani_2025|Alzahrani et al., 2025]]).

### 2.2 Performance Benchmarks

These sources provide a definitive benchmark range for the B2B Ledger NFR specification:

| Context | Protocol | TPS | Latency |
|---|---|---|---|
| Permissioned LAN (7 nodes) | BFT-SMART (Barger) | ~2,500 | ~133ms (WAN avg) |
| Sharded (15 shards, 60 cores) | Chainspace S-BAC | 350 | 69–210ms |
| Cross-chain (DSM) | Alzahrani CBOR | 250 | 5ms (optimized) |
| Geographically distributed (400 nodes) | Kauri (Berger SoK) | 4,584 op/s | N/A |
| CBDC pilot (22 nodes, 8 AWS regions) | FBFT (Benedetti) | N/A | 1.7–13s |
| Large-scale (10,000 nodes) | BECP (Abdi) | ~0.096 blocks/s | ~10s |

Key finding: **The 69ms–210ms latency range from Chainspace** is the most analogous to a real permissioned B2B ledger and should anchor the Ledger API's latency SLA. The **2,500 TPS from Barger** (Hyperledger Fabric/BFT-SMART, 7-node LAN) is the achievable upper bound for a cluster-based permissioned setup.

- **Network bottleneck law**: At $n \geq 32$ nodes, BFT performance becomes network-bound; bandwidth is the critical infrastructure investment, not CPU ([[berger_2023|Berger et al., 2023]]; [[berger_2023a|Berger et al., 2023a]]).

### 2.3 Safety & Soundness (Double-Spending Prevention)

All sources enforce state coherence through complementary mechanisms:

- **Locking via 2PC (S-BAC)**: Chainspace uses a prepare-phase lock on UTXO objects to prevent double-spending; committed objects are atomically marked inactive ([[al_bassam_2018|Al-Bassam et al., 2018]]).
- **Quorum Certificates (QC)**: Berger's SoK establishes QCs as the canonical safety primitive, ensuring no two different blocks can commit for the same slot, directly preventing equivocation and double-spend ([[berger_2023b|Berger et al., 2023b]]).
- **UTXO + Transaction Tree (Daml)**: Bernauer's Daml enforces immutability by archiving old contract instances and creating new ones atomically within a Transaction Tree; non-consuming choices reduce contention in the UTXO model ([[bernauer_2021|Bernauer et al., 2021]]).
- **Deterministic finality (FBFT)**: Benedetti's CBDC protocol achieves zero fork probability through PBFT invariants and FROST threshold signatures, making forks cryptographically impossible ([[benedetti_2022|Benedetti et al., 2022]]).

### 2.4 Compliance & Auditability

- **Signed Merkle checkpoints (Chainspace)**: Supports partial audits in $O(s + \log N)$ time and full audits by re-executing history; ZK-SNARKs enable privacy-preserving verification ([[al_bassam_2018|Al-Bassam et al., 2018]]).
- **MSP integration (Barger/Fabric)**: The Membership Service Provider (MSP) enforces identity-based access control; $2F+1$ signatures in block metadata enable external audit of BFT protocol adherence ([[barger_2021|Barger et al., 2021]]).
- **Smart-contract-native compliance (Daml)**: Signatory/Observer model enforces authorization at the language level; content-based addressing (cryptographic package hashes) ensures verifiable shared logic ([[bernauer_2021|Bernauer et al., 2021]]).
- **Cross-chain governance (Alzahrani)**: The Bridge Layer enforces compliance by ensuring all data transfers across chains adhere to pre-defined governance rules and AES-256/ECDSA security standards ([[alzahrani_2025|Alzahrani et al., 2025]]).
- **ISO 20022 Gap**: None of the Block 01 sources explicitly reference ISO 20022. This is a known gap to fill in subsequent blocks with compliance-focused sources (e.g., chuen_2017, flamini_2021).

---

## 3. Emerging NFR Requirements (Block 01 Provisional)

Based on Block 01, the following NFR candidates are provisionally identified for the B2B Ledger:

1. **NFR-AVAIL-01**: The ledger cluster MUST maintain availability with at least $3F+1$ nodes, tolerating $F$ Byzantine failures, with automated view-change completing in under 30 seconds.
2. **NFR-PERF-01**: End-to-end transaction latency MUST be under 210ms under normal load for intra-cluster operations (derived from Chainspace baseline).
3. **NFR-PERF-02**: The ledger MUST sustain a minimum of 2,500 TPS in a 7-node LAN configuration (derived from Barger/BFT-SMART baseline).
4. **NFR-SAFE-01**: State coherence MUST be enforced through Quorum Certificates or equivalent finality mechanism, making double-spending cryptographically impossible.
5. **NFR-AUDIT-01**: Every committed block MUST include a Membership-verifiable quorum certificate ($\geq 2F+1$ signatures) to enable independent audit of BFT protocol adherence.

---

## 4. Block Processing Log

| Block | Sources Processed | Status |
|---|---|---|
| Block 01 | abdi_2025, al_bassam_2018, alzahrani_2025, barger_2021, benedetti_2022, berger_2023, berger_2023a, berger_2023b, bernauer_2021 | Complete |
| Block 02 | chan_2018 ... frey_2024 | Pending |
| Block 03 | georgiou_2023 ... muratov_2018 | Pending |
| Block 04 | nasir_2022_batch03 ... sonnino_2021 | Pending |
| Block 05 | trestioreanu_2021 ... zhong_2025 | Pending |
