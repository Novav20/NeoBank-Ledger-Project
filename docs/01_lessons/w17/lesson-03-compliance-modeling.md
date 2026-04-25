# Lesson 03: The Data Blueprint (Domain Model vs. ERD)

## Introduction
In high-integrity engineering, we don't just "design tables." We design a **Domain Model** to represent business logic and an **ERD** to represent physical storage.

---

## 1. The Mechatronics Analogy: The "CNC Machine"

### The Domain Model (The "Conceptual Machine")
Think of the **G-Code** and the **Tool Paths**.
- It defines *what* the machine is doing (Cut, Drill, Move).
- It doesn't care if the motor is AC or DC; it only cares about the **Logic of the Motion**.
- **In our Ledger**: This is where concepts like **"Transaction"**, **"Double-Entry"**, and **"Account"** live.

### The ERD (The "Hardware Components")
Think of the **Wiring Diagram** and the **Database of Spare Parts**.
- It defines *how* the data is bolted to the disk.
- It cares about **Primary Keys**, **Foreign Keys**, and **Data Types** (SQL/LevelDB).
- **In our Ledger**: This is where we define the `BigInt` for money and the `String(52)` for the UTI.

---

## 2. Why we need BOTH in a Financial Ledger

### Step A: The Domain Entity Diagram
We use this to satisfy **ISO 20022**. The standard doesn't tell you to use a "Table." It tells you that a `CashAccount` relates to a `Currency`.
- **Goal**: Align with business vocabulary (Aggregates).
- **Benefit**: If we change from SQL to LevelDB, the **Domain Model** stays the same, even if the **ERD** changes completely.

### Step B: The ERD (Entity-Relationship Diagram)
We use this to satisfy **MiFID II** and **NFRs**. 
- **Goal**: Physical integrity and performance.
- **Mandatory Attributes extracted from Laws**:
  - **Timestamp (MiFID II)**: Must be 1ms precision.
  - **Amount (NFR-004)**: Must be an Integer.
  - **IDs (ISO 20022)**: Must support 52-character alphanumeric UTIs.

---

## 3. The Event-Sourced Flow
Because we are building an **Event-Sourced** ledger, our "Tables" relate differently:
1. **Command** (Intent) $\rightarrow$ **Validation** $\rightarrow$ **Event** (The Fact).
2. **Event** $\rightarrow$ updates $\rightarrow$ **JournalEntry** (Double-Entry).
3. **JournalEntries** $\rightarrow$ summed into $\rightarrow$ **Account Balance** (The Projection).

---

## Summary for Today
| Artifact | Focus | Key Constraint |
| :--- | :--- | :--- |
| **Domain Model** | Logic & Standards | ISO 20022 Metamodel. |
| **ERD** | Storage & Compliance | MiFID II Timestamp precision. |
| **API Contract** | Intake & Auth | GDPR Data Minimization. |
