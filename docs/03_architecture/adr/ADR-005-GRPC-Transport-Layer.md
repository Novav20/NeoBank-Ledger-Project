# ADR-005: gRPC Transport Layer
**Status:** Accepted
**Date:** 2026-04-27
**Deciders:** Gemini CLI / Juan David
**Supersedes:** None

---

## Context
The NeoBank Ledger needs a transport layer that can sustain low-latency, high-throughput communication across sharded BFT nodes while also supporting partner integrations in a regulated fintech environment.

The deep research synthesis for Week 18 shows that the ledger's transport path must minimize serialization overhead, avoid protocol-level blocking under load, support bidirectional message flow for consensus traffic, and provide strong identity guarantees. The same synthesis also shows that the core consensus and shard-bridge workload is more compatible with binary framing and persistent streams than with request/response APIs designed for public web traffic.

This decision remains compatible with Clean Architecture because transport stays outside the domain core and is treated as an infrastructure concern.

## Decision
We will adopt gRPC over HTTP/2 with Protocol Buffers and mutual TLS as the primary transport layer for inter-node communication and partner-integration endpoints.

## Consequences

### Positive
- Binary serialization reduces payload size and parsing overhead compared with text-based formats.
- HTTP/2 multiplexing and gRPC bidirectional streaming support consensus traffic that benefits from persistent, full-duplex channels.
- Native mTLS supports authenticated node-to-node communication and aligns with a fintech security posture that expects certificate-based identity.
- The transport layer remains consistent with the Clean Architecture boundary established in ADR-004.

### Negative / Trade-offs
- Operational complexity increases because every node needs certificate lifecycle management.
- Schema discipline becomes mandatory because .proto contracts must be versioned carefully.
- Debugging is less transparent than with JSON over plain HTTP.

### Risks
- HTTP/2 still rides on TCP, so packet loss can still introduce residual head-of-line blocking.
- If certificate issuance or rotation fails, transport availability can be affected.
- If the protocol contract drifts from the consensus message model, integration friction will increase quickly.

## Alternatives Considered
| Alternative | Reason Rejected |
|---|---|
| REST/JSON | Text parsing overhead, request/response coupling, and lack of native streaming make it a poor fit for validator traffic. |
| WebSockets | Efficient for raw streaming, but it lacks strict service contracts and built-in security semantics for a regulated B2B ledger. |
| SignalR | Useful for real-time UI delivery, but it adds abstraction overhead and fallback behavior that are not appropriate for consensus traffic. |

## References
- [02-grpc-deep-research-results.md](../../02_analysis/research/w18/02-grpc-deep-research-results.md)
- [ADR-004-Adoption-of-Clean-Architecture.md](ADR-004-Adoption-of-Clean-Architecture.md)
- [chan_2018.md](../../02_analysis/research/w13/batch-03/chan_2018.md)
- [berger_2023.md](../../02_analysis/research/w13/batch-03/berger_2023.md)
- [liao_2026.md](../../02_analysis/research/w13/batch-02/liao_2026.md)
- [Part-4-8-Serialization.md](../../02_analysis/research/w17/ISO-20022-n-MiFID-II-Extractions/Part-4-8-Serialization.md)
- [nguyen_2021.md](../../02_analysis/research/w13/batch-03/nguyen_2021.md)
- [allam_2026.md](../../02_analysis/research/w13/batch-02/allam_2026.md)