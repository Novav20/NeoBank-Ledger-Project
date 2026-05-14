# Modernize Traceability and Reorganize Documentation

## Executive Summary
I reorganized the documentation into month-based plan and weekly-log folders, renamed the requirement traceability base to `requirements.base`, added the new traceability metadata fields to functional requirements and user stories, and refactored `Traceability.md` into an embedded dashboard.

## Analysis / Findings
- The prompt wording for the timeline artifact reads more like a Gantt-style milestone diagram than an Obsidian Base table view.
- I inlined the Mermaid Gantt diagram directly in `Traceability.md` so the dashboard keeps the timeline in the single dashboard file.
- The new traceability fields are now available for matrix views, with `implementation_path`, `test_path`, and `sprint` added to the functional requirements and backlog stories.

## Proposed Plan / Solution
- Keep `Traceability.md` as the single dashboard entry point.
- Embed the milestone timeline plus the requirements and user-story matrices at the top of the dashboard.
- Preserve the monthly hierarchy under `docs/00_meta/plans/` and `docs/00_meta/orchestration/logs/weekly/`.

## Technical Impact
- **Affected Files:** `docs/00_meta/Traceability.md`, `docs/02_analysis/requirements/requirements.base`, `docs/02_analysis/requirements/user_stories.base`, `docs/00_meta/plans/05/w20.md`, `docs/00_meta/orchestration/logs/session-state.md`, `docs/00_meta/orchestration/logs/weekly/05/w20/2026-05-11.md`, `docs/00_meta/orchestration/logs/weekly/05/w20/2026-05-12.md`, functional requirement YAML files, user-story YAML files.
- **New Dependencies:** None.
- **Risk Level:** Low.

## Deliverables / Snippets
- `docs/00_meta/Traceability.md`
- `docs/02_analysis/requirements/requirements.base`
- `docs/02_analysis/requirements/user_stories.base`

## Outcome
The dashboard now uses embedded views plus a Gantt-style timeline diagram. If a native Obsidian Bases timeline is required instead of Mermaid Gantt, the current structure is already isolated enough to swap the artifact later without changing the dashboard links.