---
type: extraction-note
status: draft
source: BPA_Report v1.2
related: [../bpa/BPA_Report.md, ../bpa/identified-gaps.md]
focus: actors-and-stakeholders
---

# Actors and Stakeholders Extraction

## Purpose
This note captures only the actors and stakeholders that are explicitly supported by the BPA and its cited sources. Anything unclear is moved to the BPA gap note instead of being assumed.

## Core BPMN actors for the first pass

| Actor / Stakeholder | Role in the domain | Where it comes from |
| --- | --- | --- |
| Sponsor Banks & Central Authorities | Trust anchor, regulatory interface, and platform for integration / system-of-record access | BPA Section 1; [[alfhaili_2025]]; [[huang_2024]]; [[renduchintala_2022]] |
| Fintech Corporations & TPPs | External business actor that initiates or integrates transactions through APIs | BPA Section 1; [[walker_2023]]; [[bamakan_2020]] |
| SMEs & Corporate Consumers | Business customers that use the ledger for payments, treasury, or vendor flows | BPA Section 1; [[fatorachian_2025]]; [[singh_2022]]; [[nasir_2022]] |
| Regulators & Auditors | Oversight actor for compliance, auditability, and supervision | BPA Section 1; [[boukhatmi_2025]]; [[mashiko_2025]]; [[wang_2025]]; [[mohsenzadeh_2022]]; [[sonnino_2021]]; [[noreen_2023]] |
| Platform / Fintech | Current-state operational actor in the As-Is process map | BPA Section 2 |
| Manual / Legacy System | Current-state processing environment in the As-Is process map | BPA Section 2 |
| Node / Peer / Orderer / CA roles | Technical actors if the BPMN includes system/internal swimlanes | BPA Section 1; [[rage_2022]]; [[quamara_2024]] |

## Recommended first BPMN lanes
Use these lanes first unless the specific diagram proves you need more detail.

1. External actor or client side
2. Fintech / TPP / Platform side
3. Sponsor bank / settlement side
4. Ledger system / validation side
5. Compliance or audit side, only if the flow includes a regulatory check

## What this means for BPMN drawing
- Start from the As-Is process in BPA Section 2.
- Keep business actors separate from internal technical services unless the diagram is intentionally system-heavy.
- Use the stakeholder section to name participants, but use the process section to decide the sequence.
- Do not add actors that are not supported by the BPA or its cited sources.

## Source-backed observations
- The BPA already gives a clear process arc: intent -> validation -> journaling -> settlement.
- The BPA also names the main external parties: sponsor banks, fintechs / TPPs, SMEs / corporate consumers, and regulators / auditors.
- The cited research expands the ecosystem with peer nodes, orderers, certificate authorities, and network participants, but those are usually technical actors rather than business stakeholders.

## Next step
Turn one process, such as transaction initiation or settlement, into a BPMN diagram with these lanes and then refine the use cases from the same flow.
