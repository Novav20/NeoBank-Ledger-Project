# Lesson 20.02: Integration Testing for the RocksDB Ledger

In previous projects, you might have written **Unit Tests**, where you test a single function in isolation. Today, we are moving to **Integration Testing**.

## 1. What is Integration Testing?
An Integration Test verifies that different layers of the system work together correctly with an external dependency (the **RocksDB Database**).

- **Unit Test**: Logic in isolation (Blisteringly fast).
- **Integration Test**: Code + Real Database (Real Disk I/O).

## 2. Why we need it for the Ledger
In a high-integrity ledger, we need to prove two things:

### A. Atomicity (The Unit of Work)
We must prove that if we save a Transaction, its Entries, and Balances, they either all succeed or all fail.
- **Scenario**: Save 1 Transaction + 2 Entries + 2 Balances. Verify all 5 keys exist on disk.

### B. Immutability (The Event Store)
We must prove that once an Event is written, it cannot be changed.
- **Scenario**: Attempt to overwrite an existing Event sequence. The system must throw an error.

## 3. The Clean Room Pattern
To prevent "Test Pollution" (Test A affecting Test B):
1. **Setup**: Create a unique temporary folder for RocksDB.
2. **Act**: Perform the test.
3. **Teardown**: Delete the folder after the test.

## 4. What is an Immutability Guard?
This is a software **Interlock**. Before saving an Event, the store checks if that ID already exists. If it does, it refuses the write to protect the integrity of the history.

---

**Next Step**: Which scenario should we deliberate on first? **Atomicity** (Batch writes) or **Immutability** (Protection)?
