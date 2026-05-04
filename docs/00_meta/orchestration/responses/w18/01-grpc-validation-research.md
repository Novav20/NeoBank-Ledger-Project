# gRPC Validation Research

## Executive Summary
The research corpus does not provide direct, source-backed justification for gRPC, HTTP/2, Protobuf, WebSockets, or SignalR as the ledger transport layer. What it does provide is a consistent signal that the transport path must be efficient because BFT consensus and sharded coordination become network-bound at scale. The correct conclusion from the current corpus is **Knowledge Gap**.

## Analysis / Findings
- Exact-term scans across `docs/02_analysis/research/**` did not surface source-backed matches for gRPC, HTTP/2, WebSockets, or Protobuf.
- The strongest ledger-specific evidence is about communication cost, not transport branding: HotStuff scales linearly while IBFT scales quadratically, and BFT performance becomes network-dominated at scale. See [chan_2018.md](../../../../02_analysis/research/w13/batch-03/chan_2018.md#L7) and [chan_2018.md](../../../../02_analysis/research/w13/batch-03/chan_2018.md#L21), plus [berger_2023.md](../../../../02_analysis/research/w13/batch-03/berger_2023.md#L21) and [berger_2023.md](../../../../02_analysis/research/w13/batch-03/berger_2023.md#L23).
- The parallel-chain literature points to the same issue from another angle: the hard problem is global serialization and communication overhead. PChain keeps serialization local to a shard and decouples transaction propagation from consensus. See [liao_2026.md](../../../../02_analysis/research/w13/batch-02/liao_2026.md#L7), [liao_2026.md](../../../../02_analysis/research/w13/batch-02/liao_2026.md#L21), and [liao_2026.md](../../../../02_analysis/research/w13/batch-02/liao_2026.md#L23).
- The only explicit binary transport guidance in the corpus is ASN.1 Aligned PER for inter-shard transport in the ISO 20022 serialization extraction. That supports compact binary framing, but it does not name gRPC or any RPC framework. See [Part-4-8-Serialization.md](../../../../02_analysis/research/w17/ISO-20022-n-MiFID-II-Extractions/Part-4-8-Serialization.md#L80) and [Part-4-8-Serialization.md](../../../../02_analysis/research/w17/ISO-20022-n-MiFID-II-Extractions/Part-4-8-Serialization.md#L104).
- The only direct RPC or REST-adjacent mentions I found were either outside the core ledger evidence or in a research scaffold. The generic RPC throughput reference is in a different blockchain benchmark, while MQTT and HTTP/RESTful APIs appear in an unrelated fog/health integration paper. See [nguyen_2021.md](../../../../02_analysis/research/w13/batch-03/nguyen_2021.md#L21) and [allam_2026.md](../../../../02_analysis/research/w13/batch-02/allam_2026.md#L24).
- The W18 research guide also frames the comparison around gRPC, REST/JSON, SignalR, and WebSockets, but that file is a prompt scaffold rather than source extraction evidence. It helps define the question, not answer it. See [01-grpc-deep-research-guide.md](../../../../02_analysis/research/w18/01-grpc-deep-research-guide.md#L18), [01-grpc-deep-research-guide.md](../../../../02_analysis/research/w18/01-grpc-deep-research-guide.md#L23), [01-grpc-deep-research-guide.md](../../../../02_analysis/research/w18/01-grpc-deep-research-guide.md#L38), and [01-grpc-deep-research-guide.md](../../../../02_analysis/research/w18/01-grpc-deep-research-guide.md#L39).

## Proposed Plan / Solution
1. Treat ADR-005 as unresolved from the current corpus and record the result as **Knowledge Gap**.
2. If an evidence-backed transport decision is still required, extract new sources focused specifically on gRPC vs REST vs SignalR vs message queues in permissioned or BFT-ledger settings.
3. If implementation must start before that research exists, frame gRPC as an engineering assumption chosen for binary framing and tooling, not as a conclusion derived from the current research corpus.

## Technical Impact
- **Affected Files:** [docs/00_meta/orchestration/responses/w18/01-grpc-validation-research.md](./01-grpc-validation-research.md)
- **New Dependencies:** None
- **Risk Level:** Medium

## Deliverables / Snippets
- **Verdict:** Knowledge Gap
- **What the corpus supports:** efficient transport, compact serialization, and lower communication overhead in BFT/sharded designs.
- **What the corpus does not support:** a direct, source-backed choice of gRPC over REST, SignalR, or other transport options.