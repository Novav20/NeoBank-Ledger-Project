## Executive Summary
The user-story template is now defined as a reusable markdown artifact with 4-letter epic traceability, lean frontmatter, and a Gherkin-inspired acceptance criteria table. I chose a table form over raw Gherkin blocks because it matches the repository's documentation style and keeps the story easy to scan inside Obsidian.

## Analysis / Findings
- User stories should stay business-facing: they express stakeholder value and deliverable behavior, not implementation detail.
- The existing repo already uses tables for use cases and requirements, so a story table is the lowest-friction format for the next refinement step.
- The recommended acceptance criteria structure is a table with the columns `AC ID`, `Given`, `When`, `Then`, `Related FR/NFR`, `Verification`, and `Notes`.
- The story template now carries enough metadata to link each story to its epic, use cases, FRs, and NFRs without overloading the body.

## Proposed Plan / Solution
1. Use `docs/00_meta/orchestration/templates/user-story-template.md` as the canonical story template.
2. Start the first story batch with epic `TXEN`, then `AUDT`, and only expand to the remaining epics once their coverage is ready.
3. Add a `user_stories.base` index after the first story files exist, so the Dataview view can mirror the requirement matrix pattern.

## Technical Impact
- **Affected Files:** `docs/00_meta/orchestration/templates/user-story-template.md`
- **New Dependencies:** None.
- **Risk Level:** Low.

## Deliverables / Snippets
- `type: user-story`
- `story_id`, `epic_id`, `epic_name`, `persona`, `goal`, and `business_value` frontmatter fields.
- `acceptance_format: gherkin-table` to make the intended AC style explicit.
- A single-row acceptance criteria table template with atomic, testable criteria.
