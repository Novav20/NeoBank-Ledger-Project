# ADR-005 Transport Review

## Executive Summary
The new deep research report supports approving gRPC as the transport layer, but the repository still contains older Week 18 artifacts that describe the transport decision as unresolved. The main contradiction is not in the research synthesis itself; it is between the accepted ADR and the live planning/logging trail that still says validation is in progress.

## Analysis / Findings
- The strongest evidence in the repository now favors gRPC. The deep research report explicitly centers gRPC bidirectional streaming, HTTP/2 multiplexing, and mTLS as the fit for HotStuff/PBFT-style consensus traffic and partner integration. See [02-grpc-deep-research-results.md](../../../../02_analysis/research/w18/02-grpc-deep-research-results.md#L48), [02-grpc-deep-research-results.md](../../../../02_analysis/research/w18/02-grpc-deep-research-results.md#L52), [02-grpc-deep-research-results.md](../../../../02_analysis/research/w18/02-grpc-deep-research-results.md#L86), [02-grpc-deep-research-results.md](../../../../02_analysis/research/w18/02-grpc-deep-research-results.md#L104), [02-grpc-deep-research-results.md](../../../../02_analysis/research/w18/02-grpc-deep-research-results.md#L137), [02-grpc-deep-research-results.md](../../../../02_analysis/research/w18/02-grpc-deep-research-results.md#L170), and [02-grpc-deep-research-results.md](../../../../02_analysis/research/w18/02-grpc-deep-research-results.md#L176).
- The direct contradiction is the earlier Week 18 validation response, which says the corpus is a "Knowledge Gap" and instructs that ADR-005 remain unresolved. See [01-grpc-validation-research.md](./01-grpc-validation-research.md#L4) and [01-grpc-validation-research.md](./01-grpc-validation-research.md#L15), plus [01-grpc-validation-research.md](./01-grpc-validation-research.md#L25).
- The live session state still says transport validation is in progress and that ADR-005 is an open task. See [session-state.md](../../logs/session-state.md#L8), [session-state.md](../../logs/session-state.md#L17), and [session-state.md](../../logs/session-state.md#L22).
- The Week 18 daily log says gRPC remains under active review and that ADR-005 is still open. See [2026-04-27.md](../../logs/weekly/w18/2026-04-27.md#L10), [2026-04-27.md](../../logs/weekly/w18/2026-04-27.md#L18), [2026-04-27.md](../../logs/weekly/w18/2026-04-27.md#L24), and [2026-04-27.md](../../logs/weekly/w18/2026-04-27.md#L29).
- The Week 18 plan still lists gRPC transport validation as pending research. If ADR-005 is now accepted, that line is stale and should be revised in the next handoff. See [w18.md](../../plans/w18.md#L27).

## Proposed Plan / Solution
1. Treat [01-grpc-validation-research.md](./01-grpc-validation-research.md) as superseded by the accepted ADR.
2. Update the live Week 18 handoff artifacts on the next synchronization pass so they no longer describe transport approval as pending.
3. Keep the Week 18 plan entry only if you intentionally want it to reflect ongoing verification work rather than the formal architectural decision.

## Technical Impact
- **Affected Files:** [docs/03_architecture/adr/ADR-005-GRPC-Transport-Layer.md](../../../../03_architecture/adr/ADR-005-GRPC-Transport-Layer.md), [docs/00_meta/orchestration/responses/w18/01-grpc-validation-research.md](./01-grpc-validation-research.md), [docs/00_meta/orchestration/logs/session-state.md](../../logs/session-state.md), [docs/00_meta/orchestration/logs/weekly/w18/2026-04-27.md](../../logs/weekly/w18/2026-04-27.md), [docs/00_meta/plans/w18.md](../../plans/w18.md)
- **New Dependencies:** None
- **Risk Level:** Medium

## Deliverables / Snippets
- **Decision artifact:** [docs/03_architecture/adr/ADR-005-GRPC-Transport-Layer.md](../../../../03_architecture/adr/ADR-005-GRPC-Transport-Layer.md)
- **Contradiction found:** The earlier knowledge-gap response and the live Week 18 tracking artifacts still describe transport validation as open, which conflicts with approving ADR-005.
- **No internal contradiction in the deep research report:** the research synthesis is internally consistent with the gRPC approval.