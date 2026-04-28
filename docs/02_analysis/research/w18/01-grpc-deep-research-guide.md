# Deep Research Guide: gRPC vs. Alternatives for BFT Ledgers

Use this guide to conduct a "Source-First" validation of the transport layer. This ensures that the decision for **gRPC** (or an alternative) is backed by high-authority evidence.

---

## 1. Trustworthy Source List (Search these first)
To maintain the "High-Integrity" principle of this project, focus exclusively on:
1. **Academic Repositories**: IEEE Xplore, ACM Digital Library, USENIX (ATC/OSDI).
2. **Industry Research**: Microsoft Research, Google Research (Specifically the "Stubby" and "Protobuf" papers).
3. **Ledger Benchmarks**: Hyperledger Fabric Technical Specifications, Corda Whitepapers.
4. **Performance Labs**: Benchmarks from TechEmpower or University Distributed Systems labs.

---

## 2. NotebookLM Prompt (Copy/Paste)

> **Task**: Perform a deep technical comparison between **gRPC (HTTP/2 + Protobuf)** and alternatives (**REST/JSON**, **SignalR**, **WebSockets**) for a high-integrity, sharded BFT ledger.
>
> **Focus Areas**:
> 1. **Serialization Efficiency**: Find peer-reviewed benchmarks comparing **Protobuf** vs **JSON/XML** in high-frequency financial contexts (1ms precision).
> 2. **BFT Communication**: Analyze how the $O(n^2)$ communication complexity of **PBFT** and the $O(n)$ of **HotStuff** benefit from gRPC's **Bidirectional Streaming** vs. standard Request/Response.
> 3. **Latency**: Look for data on "Tail Latency" and "Head-of-Line Blocking" in HTTP/1.1 (REST) vs. HTTP/2 (gRPC) for distributed consensus zones.
> 4. **Safety**: Identify the standard for **mTLS (Mutual TLS)** and non-repudiation signatures within the transport layer for B2B fintech integrations.
>
> **Output Format**: Create a **Comparative Matrix** that ranks each protocol based on:
> - Throughput (TPS).
> - Latency (ms).
> - Bandwidth Efficiency.
> - BFT Suitability (Streaming support).
>
> **Strict Instruction**: Provide formal citations for every performance claim (e.g., "According to [Author, Year] at USENIX...").

---

## 3. Anticipated Findings for ADR-005
- **gRPC** should win on **Binary Serialization** and **Bidirectional Streaming** (critical for "Hearbeat" and "Pre-prepare" phases in PBFT).
- **REST/JSON** should be identified as the bottleneck for inter-node communication due to text-parsing overhead.
- **SignalR** may be identified as a "Should" for the user-facing UI, but not the high-performance core.
