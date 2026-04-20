# Copilot Response: MVP Selection and Week 16 Closing Log
**Location:** docs/00_meta/orchestration/responses/w16/08-mvp-selection-and-log.md

---

## Executive Summary
I applied MoSCoW prioritization to the seven parent stories, selected the MVP around the high-integrity transaction path, and created the missing Week 16 closing log. The release boundary is now explicit: core transaction processing and audit integrity are in scope first, while projection, reconciliation, and observability remain lower-priority backlog items.

## Analysis / Findings
| Priority | Stories | Why |
| --- | --- | --- |
| Must | TXEN-001, TXEN-002, TXEN-003, AUDT-001 | These are the minimum end-to-end slices needed to prove trusted intake, deterministic processing, double-entry integrity, and audit-ready finality. |
| Should | PROJ-001 | Real-time projection gives the portfolio a visible user outcome, but it does not define whether the ledger is trustworthy. |
| Could | RECN-001, OBSV-001 | These improve resilience and operational confidence, but they are not required to demonstrate the core happy path. |
| Won't | none at the top-level story layer | The only explicit scale deferral is the shard-expansion slice inside PROJ-001, which stays sub-scope rather than becoming a release driver. |

This selection keeps the MVP narrative tight. The first portfolio release should show that the system can accept a trusted command, normalize it, process it fairly, and preserve proof-ready history. That gives the portfolio a crisp story and avoids diluting the first release with non-core scaling or operational work.

## Proposed Plan / Solution
1. Keep the Week 17 architecture baseline anchored on the Must set.
2. Treat PROJ-001 as the next visible enhancement after the core integrity path is defined.
3. Leave reconciliation and observability in the backlog as supporting slices.
4. Use the Week 16 closeout log as the handoff point into architectural design.

## Technical Impact
- **Affected Files:** `docs/00_meta/orchestration/logs/weekly/w16/2026-04-17.md`, `docs/00_meta/orchestration/responses/w16/08-mvp-selection-and-log.md`, `docs/02_analysis/requirements/user_stories/PROJ/PROJ-001.md`, `docs/02_analysis/requirements/user_stories/RECN/RECN-001.md`, `docs/02_analysis/requirements/user_stories/OBSV/OBSV-001.md`, `docs/00_meta/orchestration/logs/session-state.md`
- **New Dependencies:** None
- **Risk Level:** Low

## Deliverables / Snippets
### MoSCoW selection
| Priority | Stories |
| --- | --- |
| Must | TXEN-001, TXEN-002, TXEN-003, AUDT-001 |
| Should | PROJ-001 |
| Could | RECN-001, OBSV-001 |
| Won't | None at the top-level story layer |

### Updated YAML priorities
- `PROJ-001` -> `Should`
- `RECN-001` -> `Could`
- `OBSV-001` -> `Could`

### Release rationale
The MVP should prove the system can safely accept trusted commands, normalize and order them deterministically, and preserve double-entry integrity with audit-ready finality. Everything else is valuable, but it is secondary to demonstrating that core ledger trust path.
