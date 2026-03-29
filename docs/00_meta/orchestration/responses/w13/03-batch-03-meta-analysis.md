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

---
### UPDATE-01 (Block 02: chan_2018 → guggenberger_2022)

**Sources Processed:** chan_2018, choi_2018, chuen_2017, fan_2025_batch03, far_2025, flamini_2021, frey_2024, georgiou_2023, guggenberger_2022

#### Critical Assessment

**High Signal (Reinforces NFRs):**

- **chuen_2017 [CRITICAL]**: The only Block 02 source to explicitly cite **ISO 20022**, **ISO 8583**, **MiFID II**, **LEI/UTI identifiers**, and **Ricardian Contracts**. This is the compliance anchor for the entire Batch 03 analysis. It confirms the 1ms timestamping requirement under MiFID II for audit event ordering. It also introduces a critical operational risk NFR via Chapter 11: **open-source governance failure** (e.g., Heartbleed, Bitcoin block-size debate) is a systemic risk for any ledger relying on community-maintained dependencies ([[chuen_2017|Lee & Deng, 2017]]).
- **guggenberger_2022 [HIGH]**: The most practically grounded Fabric benchmark in the batch. **Critical finding:** LevelDB outperforms CouchDB by 3x on write throughput. Private Data Collections (PDC) cost **2–3x throughput reduction**. Leader orderer crash recovery takes ~5 seconds under RAFT. These are directly actionable configuration constraints, not theoretical bounds ([[guggenberger_2022|Guggenberger et al., 2022]]).
- **chan_2018 [HIGH]**: Provides a rigorous analytical model for BFT timer calibration. The HotStuff/IBFT crossover analysis is critical: IBFT is faster for small validator sets but fails at scale due to $O(n^2)$ message complexity. **Design implication**: if the Ledger API uses <15 validators, IBFT is viable; beyond that, linear protocols (HotStuff) are mandatory ([[chan_2018|Chan et al., 2018]]).
- **frey_2024 [MEDIUM - Critical Note]**: PCOs/CRDTs are theoretically elegant—preventing double-spending without total ordering is a significant innovation. However, the source **provides no production TPS or latency numbers**. It cites FastPay as a related benchmark but does not integrate it. For the B2B Ledger, this is a research signal, not an implementation blueprint. Its value is in informing the **conflict-avoidance layer**, not the primary consensus mechanism ([[frey_2024|Frey et al., 2024]]).
- **georgiou_2023 [MEDIUM]**: Self-stabilizing recycling is highly relevant for **long-running ledger deployments** that must handle transient faults (bit flips, memory corruption) without rollback. Stabilization in $O(\kappa)$ rounds is promising, but the $O(n^2)$ message overhead of the consensus objects it manages is a concern at scale ([[georgiou_2023|Georgiou et al., 2023]]).
- **flamini_2021 [MEDIUM]**: Cob's Synchronization Chain model and leaderless epoch reconfiguration address the shard takeover risk identified in Block 01. The broadcast weight comparison (200 MB vs 5,000 MB vs Algorand) is useful for infrastructure sizing. However, the absence of concrete TPS figures limits direct NFR translation ([[flamini_2021|Flamini et al., 2021]]).

**Low Signal (Downgraded):**

- **far_2025 [LOW]**: Digital Twins on heterogeneous blockchains is a tangential domain. The L3 ZK Rollup latency (120ms) and cross-shard TPS (>140k) figures are for a 6G/IoT context, not financial settlement. The dNFT and sovereignty framing is irrelevant to B2B Ledger compliance ([[far_2025|Far & Bamakan, 2025]]).
- **fan_2025_batch03 [LOW-MEDIUM]**: Shard-DAG targets 6G networks with 1,200-node simulations. The 2,800–3,000 TPS figure is credible for a permissioned parallel, but the dependency on PoW for intra-shard consensus is a compliance risk (energy consumption, non-deterministic finality) for B2B regulated environments ([[fan_2025_batch03|Fan et al., 2025]]).
- **choi_2018 [LOW]**: Fantom/Lachesis is a permissionless public DAG. Its leaderless design is interesting but it provides **no concrete TPS/latency figures** and was benchmarked against a general asynchronous model, not financial infrastructure. DAG-based asynchronous finality without total ordering is architecturally incompatible with the deterministic finality required for a B2B ledger ([[choi_2018|Choi et al., 2018]]).

#### Updated Synthesis: Compliance Gap Resolved

Block 02 finally provides the **regulatory compliance foundation** that Block 01 lacked. The Handbook (chuen_2017) surfaces the full compliance stack:
- **ISO 20022**: Machine-readable financial messaging standard — must be a design constraint for all inter-ledger B2B communication.
- **MiFID II + LEI/UTI**: Require 1ms-precision event timestamping and legal entity attribution for every trade event.
- **Ricardian Contracts**: The bridge between executable ledger logic and enforceable legal text — a direct complement to Daml's signatory model (Bernauer, Block 01).

**Revised Benchmark Table (Block 01 + Block 02):**

| Context | Protocol | TPS | Latency | Source |
|---|---|---|---|---|
| Permissioned LAN (7 nodes) | BFT-SMART (Fabric) | ~2,500 | ~133ms (WAN avg) | barger_2021 |
| Permissioned Fabric (production config) | RAFT + LevelDB | >1,000 | 1.2s (WAN) | guggenberger_2022 |
| Sharded (15 shards, 60 cores) | Chainspace S-BAC | 350 | 69–210ms | al_bassam_2018 |
| Cross-chain (CBOR optimized) | Alzahrani DSM | 250 | 5ms | alzahrani_2025 |
| Geographically distributed (400 nodes) | Kauri | 4,584 op/s | N/A | berger_2023 |
| CBDC pilot (22 nodes, 8 AWS regions) | FBFT (Bank of Italy) | N/A | 1.7–13s | benedetti_2022 |

**Critical Infrastructure Decision Emerging**: The Fabric RAFT config (guggenberger) shows 1.2s WAN latency at >1,000 TPS. This exceeds the 210ms anchor from Block 01 (Chainspace). This divergence implies the Ledger API will need **tiered SLAs**: sub-300ms for intra-cluster operations, tolerating up to 1.5s for WAN cross-organization settlement.

#### New NFR Candidates (Block 02)

6. **NFR-COMP-01**: All inter-ledger B2B messages MUST comply with ISO 20022 machine-readable format.
7. **NFR-COMP-02**: Every trade event MUST carry a Legal Entity Identifier (LEI) and Unique Transaction Identifier (UTI), timestamped to 1ms precision, to satisfy MiFID II auditability.
8. **NFR-RISK-01**: All third-party open-source dependencies MUST be tracked, versioned, and subject to a defined security patching SLA to mitigate open-source operational risk (Heartbleed class vulnerabilities).
9. **NFR-CONFIG-01**: Fabric deployments MUST use LevelDB (not CouchDB) for write-heavy ledger workloads. Private Data Collections SHOULD be used only when confidentiality requirements justify the 2–3x throughput penalty.
10. **NFR-PERF-03**: The consensus timer initial value ($\tau_0$) MUST be set to at least 3 standard deviations above the expected consensus round-trip time to prevent spurious view changes under network congestion.


---
### UPDATE-02 (Block 03: gunn_2019 → nguyen_2021)

**Sources Processed:** gunn_2019, hellings_2020, li_2024, liu_2024a, liu_2025, mishra_2025, muratov_2018, nasir_2022_batch03, nguyen_2021

#### Critical Assessment

**High Signal:**

- **mishra_2025 [CRITICAL]**: Proteus is the single most architecturally relevant source in the batch for the B2B Ledger's **audit and compliance pipeline**. Its peak throughput of **345,000 txn/s** (bandwidth-limited) is the highest reliable figure across all 27 sources processed. More critically, it formally distinguishes **Committed** from **Audited** finality — the first source to do so. A transaction may be committed (durable if TEEs are secure) but not yet **audited** (safe against platform compromise). For a regulated B2B ledger, audit-level finality may be the correct legal threshold. The **Code Transparency Service (CTS)** for software supply chain provenance is a direct match for the pending NFR-RISK-01 from Block 02 ([[mishra_2025|Mishra et al., 2025]]).
- **hellings_2020 [HIGH]**: Cerberus provides the strongest theoretical TPS ceiling in the dataset ($10^5$–$10^7$ TPS via sharding to $2^{14}$ shards). Its **first-pledge rule** for UTXO double-spend prevention is the cleanest permissioned-shard safety model seen. **Critical limitation**: throughput figures are derived from modeling with 100 Mbps bandwidth caps, not empirical tests. The $71B annual transactional friction elimination claim is credible context for ROI framing. ([[hellings_2020|Hellings et al., 2020]]).
- **gunn_2019 [HIGH]**: SACZyzzyva is the only source to formally prove the **impossibility of exceeding $3F+1$ without TEEs**. The 35µs/replica datacenter overhead and the 25µs/replica internet-wide overhead are the most precise latency increments per node in the entire batch. **Remote Attestation (SGX/TrustZone)** as a compliance primitive — proving counter integrity to external auditors — is novel and directly applicable to the Ledger's auditability NFR ([[gunn_2019|Gunn et al., 2019]]).
- **nasir_2022_batch03 [HIGH]**: The best systematic benchmark aggregation in the batch. Confirms FastBFT at 100,000 TPS and RapidChain at 7,380 TPS/8.7s. The explicit enumeration of the **DSS Trilemma** as a design constraint is a useful architectural framing tool. **Storage scalability** is flagged as a neglected NFR: Bitcoin's ledger exceeds 280 GB. This is directly relevant to the B2B Ledger's long-term archival strategy ([[nasir_2022_batch03|Nasir et al., 2022]]).

**Medium Signal:**

- **li_2024 [MEDIUM — New Threat Surface Identified]**: MEV (Maximal Extractable Value) front-running causes ~$200M USD annual losses in public chains. The "Fair Consensus Factory" framework is admirable, but its most honest data point is the cost: enforcing strict FIFO fair ordering in optimized Themis still yields only **750–1,000 TPS at 400ms latency** on 20 nodes. **Design implication**: for the B2B Ledger, the MEV attack surface must be acknowledged as a compliance NFR even in a permissioned context — an adversarial orderer node can reorder transactions to extract value ([[li_2024|Li & Pournaras, 2024]]).
- **liu_2024a [MEDIUM]**: The Fabric contention rate figure is critical: **up to 40% transaction abortion rate** under high contention with OCC/MVCC. This directly threatens the >1,000 TPS target from guggenberger_2022. The contention model is a key input for capacity planning under read-write conflict scenarios ([[liu_2024a|Liu et al., 2024]]).
- **liu_2025 [MEDIUM]**: MSSP's **2,654 TPS** with 14 shards is credible and empirically tested. The **space-for-time tradeoff** (8.5x storage overhead for ~3x throughput) is an honest engineering constraint. The 20–25% m-node ratio for optimal throughput is a deployable configuration recommendation ([[liu_2025|Liu et al., 2025]]).

**Low Signal (Downgraded):**

- **muratov_2018 [LOW]**: YAC/Hyperledger Iroha shows catastrophic throughput degradation at scale: **<1 proposal/s at 64 nodes**. The 20ms vote delay requirement at 64 nodes turns a "consensus parameter" into a hard scaling ceiling. This is an anti-pattern reference, not a design template. Do not use ([[muratov_2018|Muratov et al., 2018]]).
- **nguyen_2021 [LOW]**: Fantom Foundation whitepaper with 11,000 TPS on a 7-node testnet. Like choi_2018 (Block 02), this is a permissionless PoS/DAG product-paper with inadequate controlled benchmarking. **High-latency condition (300–700ms added to 1/3 of nodes) degraded TTF to 4.64s** — unacceptable for real-time B2B settlement. Do not use ([[nguyen_2021|Nguyen et al., 2021]]).

#### Updated Synthesis: Two New NFR Dimensions

Block 03 opens two new NFR dimensions not previously surfaced:

1. **Committed vs. Audited Finality** (Proteus): The B2B Ledger must formally specify whether its settlement guarantee is at commit-level or audit-level. This has direct legal implications — regulators may require audit-level finality before a transaction is considered irrevocable.

2. **MEV / Orderer Collusion** (li_2024, Liu 2024a): In a permissioned context, the entity controlling the Ordering Service (Fabric) or the primary replica (BFT) can reorder transactions with millisecond precision. Without fair-ordering enforcement, this is an exploitable attack vector. This must be addressed in the adversary model, not just the performance model.

**Expanded Benchmark Table (Blocks 01–03, ranked by practical relevance):**

| Context | Protocol | TPS | Latency | Signal | Source |
|---|---|---|---|---|---|
| TEE-backed, bandwidth-limited | Proteus (CFT+BFT) | 345,000 | 2.6× lower than BFT | High | mishra_2025 |
| Theoretical sharded ($2^{14}$ shards) | Cerberus | $10^5$–$10^7$ | N/A (modeled) | High | hellings_2020 |
| Permissioned LAN (7 nodes) | BFT-SMART (Fabric) | ~2,500 | ~133ms (WAN) | High | barger_2021 |
| Sharded, empirical (14 shards) | MSSP/PBFT | 2,654 | N/A | Medium | liu_2025 |
| Permissioned Fabric (WAN config) | RAFT + LevelDB | >1,000 | 1.2s | High | guggenberger_2022 |
| Fair-ordering (20 nodes) | Optimized Themis | 750–1,000 | ~400ms | Medium | li_2024 |

#### New NFR Candidates (Block 03)

11. **NFR-SAFE-02**: The Ledger MUST formally distinguish **Committed** (CFT-durable) from **Audited** (BFT-verified) finality states. External settlement obligations SHOULD require audit-level finality.
12. **NFR-COMP-03**: The Ordering Service MUST implement a demonstrable fair-ordering policy (FIFO or equivalent) to prevent MEV-class attacks by adversarial ordering nodes.
13. **NFR-STORE-01**: The ledger storage strategy MUST include pruning or epoch-checkpointing to prevent unbounded state growth; a per-node storage cap MUST be defined at design time.
14. **NFR-CONFIG-02**: Shard m-node ratio MUST be configured within the **20–25%** range to balance throughput and storage overhead, per MSSP empirical results.

## 4. Block Processing Log

| Block | Sources Processed | Status |
|---|---|---|
| Block 01 | abdi_2025, al_bassam_2018, alzahrani_2025, barger_2021, benedetti_2022, berger_2023, berger_2023a, berger_2023b, bernauer_2021 | Complete |
| Block 02 | chan_2018, choi_2018, chuen_2017, fan_2025_batch03, far_2025, flamini_2021, frey_2024, georgiou_2023, guggenberger_2022 | Complete |
| Block 03 | gunn_2019, hellings_2020, li_2024, liu_2024a, liu_2025, mishra_2025, muratov_2018, nasir_2022_batch03, nguyen_2021 | Complete |
| Block 04 | praveen_2024, ren_2018, sonnino_2021, trestioreanu_2021, wang_2024a, wang_2026, yu_2023_batch03, zhao_2024, zhen_2024, zhong_2025 | Complete |

---
### UPDATE-03 (Block 04/Final: praveen_2024 → zhong_2025)

**Sources Processed:** praveen_2024, ren_2018, sonnino_2021, trestioreanu_2021, wang_2024a, wang_2026, yu_2023_batch03, zhao_2024, zhen_2024, zhong_2025

#### Critical Assessment

**High Signal:**

- **sonnino_2021 [CRITICAL — Apex Source]**: FastPay is the strongest applied performance evidence in the entire 37-source corpus. **160,000 TPS** on 48 physical cores, **<100ms intra-continental**, ~200ms US-to-Europe — empirically measured, not modeled. This is the only source that simultaneously (a) cites **ISO 20022** explicitly as the target standard for high-performance financial infrastructure, (b) frames the system as a **Real-Time Gross Settlement (RTGS)** mechanism eliminating counterparty risk, and (c) provides a full auditing framework (signed hash-chain checkpoints, partial + full audit paths). The Byzcuit/Chainspace sharded layer adds 1,550 TPS/10-shard and <1s latency as a more conservative permissioned-shard baseline. This source must anchor Section 5 of the BPA Report ([[sonnino_2021|Sonnino, 2021]]).
- **trestioreanu_2021 [HIGH — New Threat Layer]**: SPON is the only source in the entire batch that addresses the **transport-layer threat surface** for inter-ledger payments. BGP hijacking — where an ISP or AS-level adversary re-routes payment traffic — is a real, documented attack vector for B2B financial systems. A **33% latency improvement at 5% packet loss** via redundant overlay routing is meaningful in degraded network conditions. **Critical limitation**: tested only at 15 Mbps bandwidth, which is orders of magnitude below production B2B requirements. The architectural concept (intrusion-tolerant overlay + ILP) is valid; the benchmark numbers require extrapolation ([[trestioreanu_2021|Trestioreanu et al., 2021]]).
- **praveen_2024 [HIGH — Formal Verification Standard]**: Moonshot is the only BFT protocol in the batch with machine-checked safety proofs via IVy/Z3 — 190 inductive invariants confirmed fork-free across 23 monitors. The Direct Commit rule (two adjacent QCs → immediate finality) is a clean protocol design for the B2B Ledger's deterministic finality requirement. **1 block per network hop** throughput is an elegant per-unit metric. The protocol's weakness: no empirical TPS figure, only hop-normalized throughput. Useful as design intent + safety proof template ([[praveen_2024|Praveen et al., 2024]]).
- **wang_2026 [HIGH — Deployable Configuration Formula]**: The SPSN algorithm is the only source providing a **closed-form, empirically-grounded formula** for determining the optimal shard count. For 1,000 nodes with PBFT: **k=7** is optimal. For 10,000 nodes with PBFT: **k=9**. For Raft/2PC configurations the optimal is higher (k=15–16). The 100-nodes-per-shard minimum and the system failure probability constraint ($P_f \leq \eta$) are directly usable design inputs. The post-quantum flag (SIKE/SIDH acceleration required) is the first mention in the batch — an emerging NFR ([[wang_2026|Wang et al., 2026]]).

**Medium Signal:**

- **zhao_2024 [MEDIUM — Compliance Dimension: GDPR/Redactability]**: Redactable blockchains address the regulatory conflict between **immutability** (financial audit requirement) and **right to erasure** (GDPR Article 17 / Ley 1581 in Colombia). Concordit's chameleon hash approach with device-binding UDI is the most governance-aware mechanism in the batch. The 10s maximum delay for redaction finalization is acceptable. **Critical caveat**: redactable ledgers are incompatible with the immutability assumption underlying most NFR-SAFE-* requirements above — this is a genuine design conflict that must surface in the ADR ([[zhao_2024|Zhao et al., 2024]]).
- **zhong_2025 [MEDIUM — Data Governance]**: The six-stage integration pipeline's **purpose logs** (graded authorization per query), **lineage hashes** (SHA-256 chain of source-to-output), and **portable evidence bundles** are directly applicable to the Ledger API's data governance layer. The 12.9s per-1000-records latency is too slow for real-time settlement but acceptable for batch audit reporting. ISO-formatted timestamps and FD pass-rate validation (e.g., firm_id ↔ registration_date consistency) are useful compliance primitives ([[zhong_2025|Zhong et al., 2025]]).

**Low Signal (Downgraded):**

- **ren_2018 [LOW]**: "Unbounded throughput" via implicit consensus is theoretically interesting but fundamentally incompatible with the B2B Ledger's requirement for **synchronous, auditable, globally-ordered settlement**. The self-interest proof mechanism assumes rational actors — unacceptable in an adversarial regulated environment ([[ren_2018|Ren et al., 2018]]).
- **yu_2023_batch03 [LOW]**: OverShard achieves 2,344 TPS via PoW simultaneous mining — the PoW dependency is an immediate disqualification for energy-constrained B2B compliance. The virtual accounts model is conceptually interesting but not directly applicable ([[yu_2023_batch03|Yu et al., 2023]]).
- **wang_2024a [LOW]**: Hedera/Hashgraph hybrid sharding is conceptual modeling with no empirical TPS figures. The $O(B/s)$ bandwidth reduction per shard is valid theoretically but not tested ([[wang_2024a|Wang et al., 2024]]).
- **zhen_2024 [LOW]**: DSSBD achieves 128.92 TPS with **63.72s confirmation latency**. The crowdsourcing domain and RL-based parameter tuning are interesting research directions but the latency is an order of magnitude above acceptable for B2B settlement ([[zhen_2024|Zhen et al., 2024]]).

#### Final Synthesis: Batch 03 Closed

The 37-source corpus converges on a clear, multi-layer NFR architecture for the B2B Ledger:

**Performance Tier (empirically anchored):**
- FastPay (sonnino_2021): 160,000 TPS / <100ms — aspirational upper bound
- Proteus (mishra_2025): 345,000 TPS / 2.6x lower than BFT — TEE-gated upper bound
- BFT-SMART/Fabric (barger_2021): 2,500 TPS / 133ms — realistic production LAN baseline
- Fabric RAFT/LevelDB (guggenberger_2022): >1,000 TPS / 1.2s WAN — conservative production WAN baseline

**Compliance Tier (standards-grounded):**
- ISO 20022 (chuen_2017, sonnino_2021): B2B financial message format
- MiFID II LEI/UTI + 1ms timestamps (chuen_2017): trade event auditability
- Committed vs. Audited finality (mishra_2025): legal settlement threshold
- GDPR/redactability conflict (zhao_2024): must be resolved via ADR

**Safety Tier (formally grounded):**
- QC-based double-spend prevention (berger_2023b, praveen_2024)
- MEV/fair-ordering threat (li_2024): permissioned context risk
- BGP hijacking / inter-ledger transport (trestioreanu_2021): infrastructure risk

**Configuration Tier (deployable formulas):**
- PBFT 1,000 nodes → k=7 shards (wang_2026)
- LevelDB over CouchDB → 3x write throughput (guggenberger_2022)
- IBFT viable up to ~15 validators; beyond → HotStuff (chan_2018)
- 20–25% m-node ratio for sharded storage (liu_2025)
- τ₀ ≥ 3σ for consensus timer (chan_2018)

#### Final NFR Candidates (Block 04, NFRs 15–19)

15. **NFR-PERF-04**: The Ledger MUST target sub-100ms intra-cluster settlement latency under normal load, aligned with the FastPay empirical baseline (sonnino_2021), with a tiered WAN ceiling of 1.5s.
16. **NFR-COMP-04**: The Ledger API MUST frame all high-value B2B settlement flows as **RTGS** (Real-Time Gross Settlement), eliminating net-settlement credit exposure.
17. **NFR-INFRA-01**: The inter-ledger communication path MUST use an intrusion-tolerant overlay (multi-path routing) to mitigate BGP hijacking and AS-level network partition risks.
18. **NFR-SAFE-03**: The consensus protocol SHOULD be formally verified (IVy-equivalent, Z3 SMT) against fork-freedom and double-spend invariants prior to production deployment.
19. **NFR-COMP-05 (Emerging)**: The Ledger architecture MUST document a strategy for **post-quantum cryptographic migration**, with hardware acceleration targets for SIKE/SIDH as a long-term dependency.

---

## 5. Consolidated NFR Registry (All Blocks)

| ID | Category | Statement | Primary Source(s) |
|---|---|---|---|
| NFR-AVAIL-01 | Availability | $3F+1$ nodes, BFT, view-change <30s | barger_2021, berger_2023b |
| NFR-PERF-01 | Performance | <210ms intra-cluster latency (normal load) | al_bassam_2018 |
| NFR-PERF-02 | Performance | ≥2,500 TPS in 7-node LAN | barger_2021 |
| NFR-PERF-03 | Performance | τ₀ ≥ 3σ consensus timer | chan_2018 |
| NFR-PERF-04 | Performance | <100ms target, 1.5s WAN ceiling (tiered SLA) | sonnino_2021, guggenberger_2022 |
| NFR-SAFE-01 | Safety | QC-enforced finality, double-spend impossible | berger_2023b, benedetti_2022 |
| NFR-SAFE-02 | Safety | Committed vs. Audited finality formally distinguished | mishra_2025 |
| NFR-SAFE-03 | Safety | Formal verification (IVy/Z3) of fork-freedom | praveen_2024 |
| NFR-AUDIT-01 | Audit | $2F+1$ QC per block, independently auditable | barger_2021 |
| NFR-COMP-01 | Compliance | ISO 20022 inter-ledger messaging | chuen_2017, sonnino_2021 |
| NFR-COMP-02 | Compliance | LEI + UTI + 1ms timestamp per trade event (MiFID II) | chuen_2017 |
| NFR-COMP-03 | Compliance | Fair-ordering policy on Ordering Service (MEV mitigation) | li_2024 |
| NFR-COMP-04 | Compliance | RTGS settlement model (no net-settlement exposure) | sonnino_2021 |
| NFR-COMP-05 | Compliance | Post-quantum cryptographic migration roadmap | wang_2026 |
| NFR-RISK-01 | Risk | OSS dependency SLA + CVE patching policy | chuen_2017 |
| NFR-CONFIG-01 | Config | LevelDB over CouchDB; PDC only if justified | guggenberger_2022 |
| NFR-CONFIG-02 | Config | 20–25% m-node shard ratio | liu_2025 |
| NFR-CONFIG-03 | Config | PBFT k=7 shards at 1,000 nodes | wang_2026 |
| NFR-STORE-01 | Storage | Epoch checkpointing; per-node cap defined at design time | nasir_2022_batch03 |
| NFR-INFRA-01 | Infra | Multi-path intrusion-tolerant inter-ledger overlay | trestioreanu_2021 |

