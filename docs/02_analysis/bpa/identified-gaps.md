---
type: gap-note
status: open
related: [BPA_Report.md, ../requirements/bpmn/actors-stakeholders.md]
focus: actors-and-stakeholders
---

# Identified Gaps: Actors and Stakeholders

## Open questions that must stay unresolved until confirmed

| Question | Why it matters |
| --- | --- |
| Should the customer / end-user appear as a distinct BPMN actor, or is the Fintech / TPP the only external business actor in the first model? | This changes the lane structure and the start event ownership. |
| Should KYC / AML be modeled in the BPMN at all, given that customer onboarding is out of scope but the BPA still mentions identity and verification friction? | This affects whether the process starts before or after onboarding. |
| Is the Sponsor Bank an always-present lane, or only a settlement / reconciliation participant? | This affects the diagram’s main business boundary. |
| Should peer nodes, orderer nodes, and certificate authorities appear as separate BPMN participants, or should they remain internal technical services? | This determines whether the diagram is business BPMN or system BPMN. |
| Are regulators and auditors part of the operational process flow, or only a post-process review lane? | This affects whether compliance is drawn inline or as a side process. |

## Gap rule for future extraction
If a participant or handoff is not explicitly supported by the BPA or its cited sources, put it here as a question instead of inventing it.

## Current conclusion
The BPA is enough to identify the main external actors, but it is not yet precise enough to fully resolve the customer/KYC boundary and the exact treatment of technical ledger participants in BPMN.

## Open questions for process extraction

| Question | Why it matters |
| --- | --- |
| Should the first BPMN model include both As-Is and To-Be, or only the To-Be process? | This changes whether the initial diagram is descriptive, prescriptive, or both. |
| Is "reconciliation" a separate process, or just an exception path inside settlement? | This changes whether it gets its own BPMN lane or end state. |
| Should "batch file upload" be modeled as a separate subprocess in the As-Is flow? | This affects whether the legacy process is one diagram or two linked diagrams. |
| Does the future-state EOV flow replace the As-Is lifecycle, or should both be kept side by side? | This determines the documentation structure for requirements and BPMN. |
