# SCRUM FRAMEWORK: ENTERPRISE EDITION (ISO/IEC 29110 ALIGNED)

This framework defines how we move from an **Idea** to **Commissioning** using iterative cycles. It is designed to meet professional standards for small software entities.

## 1. THE LIFECYCLE PHASES
In professional Scrum, we don't do "Analysis Phase" then "Execution Phase." Instead, we do **Refinement** every week.

| Phase | Name | Goal | ISO 29110 Alignment |
| :--- | :--- | :--- | :--- |
| **01** | **Discovery** | Define "What" and "Why" (BPA). | Project Initiation |
| **02** | **Preparation** | Backlog Refinement & Architecture (ADRs). | Software Requirements |
| **03** | **Execution** | Iterative Sprints (Build/Test/Review). | Software Construction |
| **04** | **Transition** | CI/CD, Final QA, and Documentation. | Software Integration |
| **05** | **Closing** | Handover, Commissioning, and Demo. | Product Delivery |

## 2. THE ARTIFACTS (THE "TRUTH")
- **Product Backlog**: The "Source of Truth." A list of **User Stories** (Functional) and **NFRs** (Technical).
- **Definition of Done (DoD)**: The quality gate. A task is not "Done" unless it has:
    1. Code that passes all tests.
    2. Updated documentation (ADRs/Lessons).
    3. Peer/AI Review.
- **Sprint Backlog**: The "Commitment" for the current week.

## 3. THE CEREMONIES (THE "FLOW")
1. **Sprint Planning (Monday)**: Select stories from the Backlog. Define the **Sprint Goal**.
2. **Backlog Refinement (Ongoing)**: Taking "Epics" from the BPA and breaking them into small "User Stories" with **Acceptance Criteria (AC)**.
3. **Daily Sync (Daily)**: 15-minute check. What did I do? What will I do? What are my blockers?
4. **Sprint Review (Friday)**: Demonstrate the "Increment" (The working code).
5. **Sprint Retrospective (Friday)**: How can we work better next week?

## 4. FROM IDEA TO COMMISSIONING (THE ORDER)
1. **Identify Need** -> Captured in BPA.
2. **Define Requirements** -> BPA NFRs + User Stories (Week 15).
3. **Design Architecture** -> ADRs + HLD (Week 15).
4. **Iterative Build** -> Sprints (Week 16-21).
5. **Final Validation** -> User Acceptance / Stress Tests (Week 22).
6. **Deploy & Close** -> Commissioning (June 1st).
