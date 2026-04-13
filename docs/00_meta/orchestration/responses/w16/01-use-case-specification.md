## Executive Summary
The use-case draft has been converted into a dedicated specification folder at `docs/02_analysis/requirements/use_cases/`. The new spec contains detailed tables for UC-01 through UC-05, and a separate gap note captures behavior that is still unsupported by the BPA, BPMN notes, and supporting research.

## Analysis / Findings
- UC-01 can be grounded directly in BPA 5.2, which defines the command capture, execute, order, validate, append, materialize, and settle / observe flow.
- UC-01 preconditions and postconditions are supported by BPA 4.1, especially double-entry balance, integer precision, immutability, and deterministic finality.
- UC-03 is supported by BPA 4.1 and the auditability material in the research set, which reinforces hash-chain checkpoint verification.
- UC-05 has partial support through BPA 5.2 and research on observer / monitor nodes, but the exact telemetry thresholds and alert semantics are not fully specified.
- The original use-case diagram was restored in the canonical specification file so the visual overview remains available after the folder restructure.
- Validation shard failure handling is the clearest unsupported behavior, so it was recorded in `gaps.md` instead of being invented.
- The `FR-LDR-*` entries were a premature carryover and have been removed from the use-case spec because Functional Requirements have not been derived yet.
- Associated Functional Requirements are intentionally omitted from the use-case spec for now and will be derived after the use cases are finalized and the remaining gaps are closed.

## Proposed Plan / Solution
1. Use `specifications.md` as the canonical source for FR derivation.
2. Resolve the open items in `gaps.md` before freezing the backlog.
3. Keep the spec wording business-first and map any later technical detail back to the BPA and BPMN notes.

## Technical Impact
- **Affected Files:** `docs/02_analysis/requirements/use_cases/specifications.md`, `docs/02_analysis/requirements/use_cases/gaps.md`, deleted obsolete `docs/02_analysis/requirements/use-cases.md`.
- **New Dependencies:** None.
- **Risk Level:** Medium.

## Deliverables / Snippets
- Detailed use-case specifications for UC-01 through UC-05.
- Gap register for validation failure handling, reconciliation behavior, monitoring thresholds, and audit proof delivery.
- Canonical requirements structure under `docs/02_analysis/requirements/use_cases/`.
