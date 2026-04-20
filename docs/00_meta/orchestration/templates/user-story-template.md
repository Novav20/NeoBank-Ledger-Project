type: user-story
version: 1.0
last_edited: 2026-04-15
status: draft
story_id: [TXEN-001]
epic: [TXEN|PROJ|AUDT|RECN|OBSV]
epic_id: [TXEN]
epic_name: [Transaction Engine (EOV)]
title: [Short story title]
persona: [Fintech Partner | Sponsor Bank | Auditor | System Operator]
goal: [What the persona wants to achieve]
business_value: [Why this story matters]
acceptance_format: gherkin-table
source:
  - [docs/02_analysis/requirements/epics.md#TXEN]
  - [docs/02_analysis/bpa/BPA_Report.md#1.3 Stakeholders & Ecosystem]
derived_from:
  - [docs/02_analysis/requirements/epics.md#TXEN]
related_use_cases:
  - [UC-XX]
related_fr:
  - "[[FR-XXX]]"
related_nfr:
  - "[[NFR-XXX]]"
depends_on: []
priority: [Must|Should|Could|Won't]
estimate: [1|2|3|5|8|13]
verification:
  - [acceptance test]
risk_level: [Low|Medium|High|Critical]
notes: []

# [Story Title]

## Story Statement
As a [persona], I want [goal], so that [business_value].

## Context
[Explain the business situation, the user need, and why this story belongs to the epic.]

## Acceptance Criteria
Use a Gherkin-inspired table. Keep one row per atomic criterion. Verification is captured in frontmatter, so keep the table behavior-only.

| AC ID | Given | When | Then | Notes |
| --- | --- | --- | --- | --- |
| AC-001 | [precondition] | [trigger] | [expected outcome] | [optional] |

## Dependencies
- [List upstream stories, epics, or requirements that must exist first.]

## Notes
[Capture assumptions, exclusions, open questions, or implementation hints here.]
