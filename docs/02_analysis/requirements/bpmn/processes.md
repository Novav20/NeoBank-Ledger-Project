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

## First BPMN process sketch (As-Is transaction handling)

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
