# Lesson 19.05: Event Sourcing and the Monotonic Sequencer

In a traditional database, you store the *current state* (e.g., "Balance is $100"). In a **Ledger with Event Sourcing**, you store the *events* that led to that state (e.g., "+$50, +$50"). 

To make this work, **Ordering is everything**.

## 1. Why Monotonic Sequencing?
If two events happen at the same time, the system must decide which one came first. A **Monotonic Sequence** is a series of numbers that only ever increases (1, 2, 3...).

### The Ledger Rules (BPA 5.2)
1. **No Gaps**: If you see Event 1 and Event 3, but Event 2 is missing, the ledger is "Corrupt."
2. **No Re-ordering**: If you process a "Withdraw" before a "Deposit," the transaction might fail due to insufficient funds, even if the total balance is fine.
3. **Deterministic Replay**: If we delete the "World State" (Balances), we must be able to replay the Events from 1 to N and arrive at the *exact same* balance every time.

---

## 2. Sequencing in a Key-Value World (RocksDB)
Unlike SQL Server, RocksDB doesn't have an `IDENTITY` column. We have to build our own "Sequencer."

### The "Metadata Key" Strategy
We reserve a special key in the database: `meta:last_sequence`.

**The Flow:**
1. **Initialization**: When the system starts, it reads `meta:last_sequence`. If it's empty, we start at 0.
2. **The Request**: A new Transaction arrives.
3. **Increment**: The system takes `LastSequence + 1`.
4. **Atomic Save**: We use a `WriteBatch` to save:
   - The new **Event** at key `evt:0000...002`.
   - The updated **Metadata** at key `meta:last_sequence` with value `2`.

By saving the counter *inside* the same batch as the event, we guarantee that the counter only moves forward if the event is successfully written.

---

## 3. The Sequencer as a Bottleneck
In a high-speed system, having one "Global Counter" can be a bottleneck because only one thread can increment it at a time. 

**Our Solution for MVP**: 
We will use C# `Interlocked.Increment` for thread-safe memory updates and the RocksDB `WriteBatch` for disk persistence. This allows us to handle thousands of events per second on a single node.

---

## 4. The Event Store Interface
The Application layer will use the `IEventStore` interface. It doesn't care *how* the number is generated; it just wants to `Append` an event and get the final record back.

```csharp
public interface IEventStore {
    Task<Event> AppendAsync(Transaction transaction);
    Task<IEnumerable<Event>> GetSequenceRangeAsync(long start, long end);
}
```

**Next Step**: Review **ADR-010** to see how we formalize this "Metadata Key" choice before implementation.
