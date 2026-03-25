# Glossary: Mechatronics to FinTech Bridge

## Core Concepts
- **Ledger**: The "State Machine" of the financial system. It's not just a database table; it's a sequence of confirmed states. Like a PLC's logic scan, each transaction is a state transition that must be validated.
- **Double-Entry Bookkeeping**: A "Closed-Loop Control System" for money. For every action (Debit), there is an equal and opposite reaction (Credit). The system's "Set Point" is always zero (Debits - Credits = 0). This is the fundamental error-checking mechanism.
- **Banking-as-a-Service (BaaS)**: A "Modular Actuator" model. Instead of one giant machine doing everything (monolithic bank), you have specialized actuators (Fintechs) for specific tasks (payments, lending), all powered by a common, regulated energy source (the Sponsor Bank's charter).

## Key Entities & Actors
- **Sponsor Bank**: The "Power Supply Unit (PSU)" of the ecosystem. It provides the regulated electricity (the bank charter and access to payment rails) that allows everything else to function.
- **Fintech / TPP (Third-Party Provider)**: A specialized "End-Effector" or "Tool" attached to the main robotic arm. It performs a specific task, like gripping (Payments) or welding (Lending).
- **Journal Entry**: The "Log File" for a single state transition. It contains the balanced set of Debits and Credits that describe one financial event.
- **Settlement**: The "Commit to Flash Memory" or "EPROM Burn." It's the point of no return where a transaction is finalized and becomes a permanent part of the historical state.

## Technical & Architectural Principles
- **Immutability**: A "Write-Once, Read-Many (WORM)" memory system. Like burning a final program to a chip, once a transaction is written to the ledger, it cannot be altered, only reversed with a new, compensating transaction. This is critical for auditing.
- **Nth-Party Risk**: "Cascading Failure." The risk that one faulty component (a TPP) in a complex system of interconnected parts can cause the entire system to fail, even if your direct connection to it is secure.
- **ACID Properties**: The "Safety Interlocks" of a database transaction.
    - **Atomicity**: The entire operation (Debit AND Credit) completes, or none of it does. You can't have a half-finished weld.
    - **Consistency**: The system moves from one valid state to another, always enforcing the "Debits = Credits" rule.
    - **Isolation**: Two transactions running at the same time can't interfere with each other, like two robot arms not crashing into each other's workspace.
    - **Durability**: Once a transaction is "Settled," it survives a power failure.
- **Event Sourcing**: Storing every state change as an immutable event in a log, rather than just updating the final state. It's like having a high-speed video recording of your entire assembly line, not just a photo of the finished product.
- **CQRS (Command Query Responsibility Segregation)**: Using different data models for "Writing" (Commands, optimized for speed) and "Reading" (Queries, optimized for analytics). It's like having a separate, high-speed input for your CNC machine's real-time control, and a different output for generating slow, detailed performance reports.
- **Directed Acyclic Graph (DAG)**: A data structure for transactions that doesn't require a single chain. Think of it as a parallel processing workflow where multiple steps can happen at once, as long as their dependencies are met, allowing for higher throughput than a single, sequential assembly line.
- **Execute-Order-Validate (EOV)**: A permissioned-ledger transaction pattern where execution/simulation happens before final ordering, and validation happens after ordering to confirm version consistency and policy rules before commit.
- **MVCC (Multi-Version Concurrency Control)**: A consistency mechanism that tracks object versions so concurrent transactions can be validated against read/write conflicts without global locking.
- **Finality Model**: The rule defining when a transaction is considered irreversible. It can be probabilistic (confidence grows over confirmations) or deterministic (immediate/absolute once committed).
- **Cross-Shard Atomicity**: Guarantee that a transaction touching multiple shards is applied everywhere or nowhere, preventing partial commits and broken balances.
- **Sequencer / Orderer**: The component responsible for producing a deterministic transaction order used by validators and auditors for replayable history.
- **Validation Shard**: A shard specialized in conflict checks, dependency verification, and cross-shard consistency before append-to-log commit.
- **State Projection (Read Model)**: A derived, query-optimized representation (e.g., balances) continuously materialized from the append-only event log.
