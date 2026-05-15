# Lesson 20.07: The Double-Entry Engine (Entities vs. Domain Services)

In this lesson, we move from the "Plumbing" (Infrastructure) to the "Brain" of the ledger. We need to decide where the most important rule of the system lives: **The Double-Entry Axiom.**

## 1. Beyond the "Rich Model" Argument
In professional architecture, we decide based on **Responsibility** and **Scope**.

### A. Entity Responsibility (Internal Invariants)
An **Entity** (like `Transaction`) is responsible for its own internal state. 
- **Rule**: "A transaction amount cannot be negative."
- **Rule**: "The Currency Code must be 3 letters."
- **Why?**: Because these rules only depend on the data *inside* that specific object. If the object allows a negative amount, it is "Corrupt" from birth.

### B. Domain Service Responsibility (Cross-Entity Orchestration)
A **Domain Service** is used when a business rule involves **multiple different entities** or requires **external coordination**.
- **Rule**: "The sum of all Entries in a Transaction must be zero."
- **Rule**: "The source Account must have sufficient funds."
- **Why?**: Because the `Transaction` entity shouldn't "know" about the current balance of an `Account`. That would make the `Transaction` too complex and "heavy."

---

## 2. The Mechatronics Analogy: The Industrial Scale
Think of an automated mixing machine in a factory:
1.  **The Container (Entity)**: Knows its own weight and capacity. It won't let you pour more than 10L. (Internal Invariant).
2.  **The Mixing Controller (Domain Service)**: It doesn't live inside the container. It looks at **Container A** and **Container B**, calculates the ratio between them, and ensures the total output matches the input.

The Container shouldn't know how to mix; it just knows how to hold liquid. The Controller handles the "Logic" of the process.

---

## 3. Our Double-Entry Workflow
To satisfy **BPA 4.1**, we will implement a hybrid flow:

1.  **`Transaction` (Entity)**: Validates that it has a valid ID, UTI, and non-negative amount.
2.  **`DoubleEntryEngine` (Domain Service)**: 
    - Receives the `Transaction`.
    - Generates the matching `Entries` (Debits/Credits).
    - **Validates the Axiom**: Performs the mathematical check: $\sum \text{Debits} + \sum \text{Credits} = 0$.
    - Returns a "Result" (Success or Rejection).

---

## 4. Key Term: "Statelessness"
Domain Services are **Stateless**. They are like a calculator: you give them inputs, they do the math, and they give you an answer. They don't store data themselves; they just provide the "Intelligence" to the Application Layer.

**Next Step**: Review **ADR-012** to see how we formally separate these responsibilities in our project contract.
