---
type: cheatsheet
topic: BPMN
tool: draw.io
focus: Process modeling for ledger requirements
---

# BPMN Cheatsheet for BPMN First Analysis

## What BPMN is for
Use BPMN when you need to make the business flow explicit before writing requirements.
In this project, BPMN helps you answer:
- What starts the process?
- Who does what?
- Where are the decisions?
- Where can the flow fail or branch?
- Which step becomes a system boundary?

## Where to extract BPMN from the BPA
Use the BPA like this:
- **Section 1**: identify actors and stakeholders.
- **Section 2**: extract the actual process steps. This is the main BPMN source.
- **Section 3**: capture gaps, exceptions, and pain points.
- **Section 4**: capture business rules, guards, and constraints that become gateways or validation steps.
- **Section 5.2**: use the To-Be flow as the target BPMN shape.
- **Sections 6-7**: use only as performance and future-work constraints, not as process steps.

## Extraction recipe
1. Pick one business goal, such as "process a transfer".
2. Mark the trigger.
3. List the actors and lanes.
4. Write the happy path in order.
5. Add validation and decision gateways.
6. Add exception paths from pain points.
7. Check that every task changes state or evidence.
8. Compare the flow with BPA Section 2 and 5.2.
9. If a step is only a rule, do not draw it as a task.
10. If a step is a decision, draw it as a gateway.

## Obsidian-friendly workflow
1. Read the BPA section.
2. Mark business events, actors, and decisions.
3. Draw the process in draw.io.
4. Link the BPMN note to the related use-case note.
5. Keep one process per note when possible.

## Draw.io conventions
- Use one pool for the main ledger process.
- Use lanes for actors or services.
- Use tasks for actions that change state.
- Use gateways for yes/no or branch decisions.
- Use message flows only between different participants.
- Use sequence flows inside the same process.
- Keep labels short and verb-based.

## Symbols to remember
| Symbol | Meaning | When to use |
| --- | --- | --- |
| Start event | Process begins | A request, trigger, or incoming message |
| End event | Process finishes | Success, rejection, or exception end |
| Task | Work step | Validate, post, notify, project |
| Gateway | Decision point | Balance check, approval, compliance check |
| Sub-process | Collapsed detail | When a step is too large for the main diagram |
| Data object | Business data | Journal entry, request payload, audit record |
| Message flow | Cross-party communication | API calls, external system handoffs |

## Ledger-oriented BPMN pattern
- Trigger enters the gateway.
- Schema and business validation happen in separate steps.
- Ordering is explicit if consensus is part of the flow.
- Commit happens only after validation.
- Projection and notification happen after commit.
- Exceptions should be visible as separate end states.

## Questions to ask while modeling
- Is this step manual or automated?
- Does this step change the ledger or only record evidence?
- Is the decision local, or does it depend on another system?
- Is this a happy path or an exception path?
- Can this step be reused in other flows?

## Common mistakes
- Mixing business validation with technical routing.
- Drawing too many lanes for the first pass.
- Hiding exception paths.
- Labeling gateways with nouns instead of questions.
- Overloading one diagram with every edge case.

## Quick template
```text
Pool: Ledger process
Lane 1: External actor
Lane 2: API gateway
Lane 3: Validation / ordering
Lane 4: Ledger / projection

Start -> Receive request -> Normalize -> Validate -> Decide -> Commit -> Project -> End
```

## Output quality checklist
- [ ] One clear process objective
- [ ] One start event
- [ ] One or more end states
- [ ] All gateways are questions
- [ ] Every exception path is visible
- [ ] The flow matches the BPA wording
- [ ] The diagram can be explained in 60 seconds
