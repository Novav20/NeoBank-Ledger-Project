---
type: extraction-note
status: draft
source: BPA_Report v1.2
related: [../bpa/BPA_Report.md, ../bpa/identified-gaps.md, ./actors-stakeholders.md]
focus: processes
---

# Processes Extraction

## Purpose
This note identifies the business processes that are explicitly supported by the BPA. It does not invent missing flows. If the BPA is unclear, the ambiguity is recorded in the gap note.

## Main BPA process family
The BPA describes one core business process family: the transaction lifecycle for the ledger. In simple terms, this is the process of getting a transaction from intent to settlement.

### Top-level process
- **Process a transaction / payment / transfer**: the umbrella business process that covers the end-to-end movement from request to final state. The BPA does not name this as a formal BPMN label, but its Section 2 lifecycle makes it the clearest first process to model.

### Lifecycle subprocesses explicitly stated in the BPA
| Step | BPA term | BPMN meaning | Source |
| --- | --- | --- | --- |
| 1 | Intent (Transaction Initiation) | The process starts when a user platform or API submits a transaction intent | BPA Section 2; [[cisar_2025]] |
| 2 | Validation (Consensus & Verification) | The system checks consensus, access controls, and transaction validity | BPA Section 2; [[quamara_2024]]; [[rage_2022]] |
| 3 | Journaling (Data Recording) | The validated transaction is written into the append-only ledger log | BPA Section 2; [[fulbier_2023]] |
| 4 | Settlement | The recorded state is finalized and becomes operationally complete | BPA Section 2; [[cisar_2025]]; [[quamara_2024]] |

## Supporting process behavior found in the BPA
- **Manual / legacy validation path**: The As-Is sequence shows manual validation and spreadsheet / legacy DB entry before upload.
- **Batch file upload**: The current-state process includes an end-of-day batch upload to the sponsor bank.
- **Reconciliation / exception handling**: The current-state process ends with reconciliation errors and exceptions when the batch result does not match.
- **Future-state execution flow**: Section 5.2 shows the technical target flow of Command & Intent Capture -> Execute -> Order -> Validate -> Append & Materialize -> Settle & Observe.

## Recommended BPMN process groups for the first pass
1. **As-Is transaction handling**: show the legacy manual flow from intent to delayed settlement.
2. **To-Be transaction lifecycle**: show the target flow from command to settlement and observation.
3. **Exception / reconciliation handling**: model only if the diagram needs to show what happens when the batch or transaction fails.

## What is not yet a confirmed process
- KYC / AML onboarding is mentioned in the BPA, but the out-of-scope section says customer onboarding is out of scope.
- Therefore, onboarding is not yet a confirmed BPMN process for this project phase.
- Regulatory review is important, but the BPA does not yet prove whether it should be a core lane or a later audit lane.

## First diagram recommendation
If you need one BPMN diagram first, model the **transaction lifecycle** rather than only "process a transfer".
That lifecycle is the process family. A transfer is one instance of it.

## Next extraction step
Use this process inventory to sketch one BPMN diagram for the As-Is flow and one BPMN diagram for the To-Be flow.

## First BPMN process sketch (Ver 1.0)

```text
Pool: Transaction handling

Lane 1: Platform / Fintech
Lane 2: Manual / Legacy System
Lane 3: Sponsor Bank

Start event: User or API submits transaction intent
1. Platform / Fintech sends unstructured payment intent
2. Manual / Legacy System performs manual data validation
3. Manual / Legacy System records the entry in spreadsheet or legacy DB
4. Manual / Legacy System checks for data issues and float errors
5. Manual / Legacy System sends batch file upload to Sponsor Bank at end of day
6. Sponsor Bank processes the batch
7. Sponsor Bank returns reconciliation errors or exceptions if data does not match
8. Manual / Legacy System notifies Platform / Fintech of delayed settlement or exception
End event: Settlement completed or exception raised
```

## How to draw it
- Put the start event in the Platform / Fintech lane.
- Put the manual validation and entry steps in the Manual / Legacy System lane.
- Put the batch upload and reconciliation steps at the Sponsor Bank boundary.
- Show the exception path as a separate end state, not only as a note.
- Keep the diagram focused on the legacy flow first; do not add the To-Be EOV steps yet.
- If the diagram starts to feel crowded, collapse steps 2-4 into a subprocess and keep the gateways visible.

## Appendix A: BPMN 2.0 refinement notes

### Purpose
This note turns the first As-Is transaction draft into a more precise BPMN 2.0 model.
It keeps the current diagram aligned with the BPA while making the event, task, and gateway types explicit.

### Refinement rules
- Use a message start event when the process is triggered by an external request.
- Use send tasks for outward handoffs.
- Use manual tasks for human-led checks.
- Use user tasks when a person works through a tool or legacy UI.
- Use service tasks for automated platform-side execution.
- Use exclusive gateways when the flow splits on a yes/no decision.
- Use separate end events for success and exception outcomes.
- If a step is really a schedule, model it as a timer event or a timer-driven task, not as a generic task.

### Current draft to BPMN 2.0 subtype mapping

| Current draft step | Recommended BPMN subtype | Why this fits better | Gateway or note |
| --- | --- | --- | --- |
| User or API submits transaction intent | Message start event | The process is triggered by an incoming request | If the model later adds a client pool, this becomes a message flow from that participant |
| Send unstructured payment intent | Send task | This is an outbound handoff from Platform / Fintech | Keep it as a verb phrase |
| Perform manual data validation | Manual task | The step is a human review, not an automated system action | Add an XOR gateway if invalid requests must stop here |
| Record the entry in spreadsheet or legacy DB | User task | The person is interacting with a tool or legacy interface | If the diagram gets crowded, this can collapse into a subprocess |
| Check for data issues and float errors | Manual task, or business rule task if the rule becomes explicit | The step is still a rule-based human check in the As-Is flow | Add an XOR gateway: "Data issues found?" |
| Send batch file upload to Sponsor Bank at end of day | Timer-driven send task, or an intermediate timer event followed by a send task | The action is scheduled, not immediate | If needed, model the schedule with a timer event before the send task |
| Process the batch | Service task | Sponsor bank processing is the clearest automated action in the current draft | This can also become a collapsed subprocess if bank internals stay out of scope |
| Return reconciliation errors or exceptions if data does not match | Exclusive gateway plus send task on the exception path | The real BPMN decision is whether the batch reconciles | Gateway question: "Batch reconciles?" |
| Notify Platform / Fintech of delayed settlement or exception | Send task | This is the outbound notification to the originating side | Reuse it for both upstream exception paths |
| Settlement completed or exception raised | Two end events | BPMN should separate success from failure | One end event for settlement complete, one end event for exception |

### Recommended gateway set for the first refinement pass

1. **Data issues found?** after manual validation and float checking.
2. **Batch reconciles?** after sponsor bank processing.
3. **Request valid?** only if the team wants an explicit reject path before the batch window.
4. **Response timed out?** only if we want to show a waiting state instead of assuming the bank always replies.

### Notes for the diagram revision
- The current draw.io file is still a first draft, so the next pass should refine symbols, not expand scope.
- Keep the Sponsor Bank lane unless we decide to turn the view into a collaboration diagram with separate pools.
- Add data objects if the diagram needs more traceability: transaction intent, batch file, reconciliation report, exception notice.
- Keep the exception path visible as a real branch, not as a footnote.

### Suggested revised flow
```text
Pool: Transaction handling

Lane 1: Platform / Fintech
Lane 2: Manual / Legacy System
Lane 3: Sponsor Bank

Start event: Message start event - transaction intent received
1. Send task: submit payment intent
2. Manual task: validate transaction data
3. User task: record entry in spreadsheet or legacy DB
4. Manual task: check data issues and float errors
5. XOR gateway: data issues found?
	- Yes -> Send task: notify platform of delayed settlement or exception -> End event: exception raised
	- No -> Timer-driven send task: upload batch file to Sponsor Bank
6. Service task: process batch file
7. XOR gateway: batch reconciles?
	- Yes -> End event: settlement completed
	- No -> Send task: notify platform of delayed settlement or exception -> End event: exception raised
```

### Open modeling choice
If the next revision needs to show cross-party communication more explicitly, split Platform / Fintech and Sponsor Bank into separate pools and convert the handoffs into message flows.
