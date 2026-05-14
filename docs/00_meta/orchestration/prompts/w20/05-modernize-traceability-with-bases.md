---
status: pending
issued_by: Gemini CLI
issued_date: 2026-05-13
response_ref: docs/00_meta/orchestration/responses/w20/05-modernize-traceability-with-bases.md
---
# Task: Modernize Traceability and Reorganize Documentation

## Task Context
We are upgrading our traceability system and repository structure for better scalability. We will leverage the native **Obsidian Bases** (Timeline & Table views) and move toward a monthly folder hierarchy for plans and logs.

## Objectives
1.  **Refactor Bases**: 
    - Rename `docs/02_analysis/requirements/traceability.base` to `docs/02_analysis/requirements/requirements.base`.
    - Create `docs/00_meta/timeline.base` using the native **Timeline** layout to track project milestones.
2.  **Monthly Folder Restructure**: Group horizontal files/folders into month-based subfolders (`03`, `04`, `05`).
3.  **Metadata Update**: Update YAML properties in requirements and user stories to support the new Bases views.
4.  **Dashboard Refactor**: Transform `docs/00_meta/Traceability.md` into a clean, embedded dashboard.

## 1. Documentation Reorganization (Monthly Hierarchy)
Move files and folders into their respective month subfolders (03=March, 04=April, 05=May):

- **Plans** (`docs/00_meta/plans/`):
  - Move `w11.md`, `w12.md`, `w13.md` -> `03/`
  - Move `w14.md`, `w15.md`, `w16.md`, `w17.md` -> `04/`
  - Move `w18.md`, `w19.md`, `w20.md` -> `05/`
- **Weekly Logs** (`docs/00_meta/orchestration/logs/weekly/`):
  - Move folders `w11/`, `w12/`, `w13/` -> `03/`
  - Move folders `w14/`, `w15/`, `w16/`, `w17/` -> `04/`
  - Move folders `w18/`, `w19/`, `w20/` -> `05/`

## 2. Obsidian Bases & Traceability
- **Metadata**: Add `implementation_path`, `test_path`, and `sprint` (e.g., `Sprint 01`) to all Functional Requirements and User Stories.
- **Timeline Base**: Configure `timeline.base` to filter for project milestones and display them using the built-in **Timeline** view.
- **Traceability Dashboard**: Refactor `docs/00_meta/Traceability.md`:
  - **REMOVE** the manual "Weekly Logs" and "Project Timeline" lists (they are now redundant).
  - **EMBED** the new bases at the top:
    - `timeline.base` (Milestone view).
    - `requirements.base` (Fulfillment matrix).
    - `user_stories.base` (Backlog matrix).

## Constraints & Requirements
- **Relative Paths**: Update all embedded paths and internal links to reflect the new month-based folder structure.
- **Clean State**: Ensure the resulting dashboard is a visual "Single Pane of Glass" for the project.

## Implementation Path
- `docs/02_analysis/requirements/*.base`
- `docs/00_meta/plans/` (Restructure)
- `docs/00_meta/orchestration/logs/weekly/` (Restructure)
- `docs/00_meta/Traceability.md`
- `docs/00_meta/timeline.base` (New)
