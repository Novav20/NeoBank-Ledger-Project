---
status: pending
issued_by: Gemini CLI
issued_date: 2026-04-27
response_ref: docs/00_meta/orchestration/responses/w18/02-adr-005-grpc-transport.md
---
# Task: ADR-005 Generation (gRPC Transport Layer)

## Task Context
Following a deep technical research phase (02-grpc-deep-research-results.md), we have identified that gRPC (HTTP/2 + Protobuf) is the only protocol that satisfies the multi-dimensional requirements of our high-integrity sharded BFT ledger. We need to formalize this decision in the architecture baseline.

## Objective
Create `docs/03_architecture/adr/ADR-005-gRPC-Transport-Layer.md` using the standard project template.

## Specific Instructions
1.  **Decision**: Adopt **gRPC (HTTP/2)** with **Protocol Buffers (Protobuf)** and **Mutual TLS (mTLS)** as the primary inter-node and partner-integration transport layer.
2.  **Rationale**:
    - **Serialization Efficiency**: Protobuf reduces payload size by 50-80% and increases serialization speed by up to 96% for large snapshots compared to JSON.
    - **BFT Optimization**: gRPC's **Bidirectional Streaming** enables linear ($O(n)$) communication complexity for HotStuff/PBFT by eliminating handshake overhead and supporting native flow control.
    - **Latency Stability**: Multiplexing over HTTP/2 reduces tail latency (P99) by up to 76% under high load, preventing "Message Storm" consensus failures.
    - **Security Compliance**: Built-in mTLS and sender-constrained tokens align with the **FAPI 2.0** security profile for fintech.
3.  **Alternatives Considered**:
    - **REST/JSON**: Rejected due to high text-parsing overhead and lack of streaming, leading to a "scalability ceiling."
    - **WebSockets**: Rejected for being "un-opinionated"; lacks the strict API contracts and built-in security profiles of gRPC.
    - **SignalR**: Rejected for high library overhead and "long polling" fallback anti-patterns in BFT contexts.
4.  **Consequences**:
    - **Positive**: Linear scaling; sub-second finality support; native compliance.
    - **Negative**: Tooling complexity; requires X.509 certificate management for all nodes.

## Constraints & Requirements
- Status: **Accepted**.
- Date: 2026-04-27.
- Deciders: Gemini CLI / Juan David.
- References: Cite `docs/02_analysis/research/w18/02-grpc-deep-research-results.md` and the academic sources listed there (e.g., Gorton, 2024; HotStuff-2, 2024; USENIX Mu, 2020).
