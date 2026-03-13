---
week: 11
date: 2026-03-13
title: Orchestration Policy Consolidation
status: Complete
---

# Consolidation: Centralized Orchestration Policy

## Summary
Moved all evolving operational content (session bootstrap, naming conventions, escalation/rejection protocol, ADR/traceability requirements) from `GEMINI.md` and `.github/copilot-instructions.md` into a single, authoritative source: `docs/00_meta/OrchestrationPolicy.md`.

## Changes Made
1. **Created** `docs/00_meta/OrchestrationPolicy.md` with:
   - Session bootstrap and live snapshot rules.
   - Naming conventions (`wNN` folders).
   - Escalation and rejection protocol.
   - ADR and traceability mandates.

2. **Updated** `GEMINI.md`:
   - Section 2 now summarizes roles and references the central policy.
   - Removed duplicate escalation/rejection details.

3. **Updated** `.github/copilot-instructions.md`:
   - Session initialization section points to `OrchestrationPolicy.md`.
   - Escalation protocol section points to the central policy.

## Rationale
- Avoid duplicating content that changes frequently (naming, session rules, protocols).
- Ensure both `GEMINI.md` and Copilot instructions reference the same rules.
- Simplify maintenance: update the policy once; both orchestration files inherit the change.

## Next Steps
- Use `OrchestrationPolicy.md` as the authoritative source for workflow questions.
- Use `GEMINI.md` for high-level role definitions and project context.
- Use `.github/copilot-instructions.md` for Copilot-specific capabilities and constraints.
